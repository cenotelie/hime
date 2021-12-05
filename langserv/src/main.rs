/*******************************************************************************
 * Copyright (c) 2020 Association Cénotélie (cenotelie.fr)
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation, either version 3
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General
 * Public License along with this program.
 * If not, see <http://www.gnu.org/licenses/>.
 ******************************************************************************/

//! Generator of lexers and parsers for the Hime runtime.

pub mod workspace;

use std::sync::Arc;

use futures::future::join_all;
use log::{error, info};
use tokio::sync::RwLock;
use tower_lsp::jsonrpc::{Error, Result};
use tower_lsp::lsp_types::*;
use tower_lsp::{Client, LanguageServer, LspService, Server};
use workspace::Workspace;

/// The name of this program
pub const CRATE_NAME: &str = env!("CARGO_PKG_NAME");
/// The version of this program
pub const CRATE_VERSION: &str = env!("CARGO_PKG_VERSION");
/// The commit that was used to build the application
pub const GIT_HASH: &str = env!("GIT_HASH");
/// The git tag that was used to build the application
pub const GIT_TAG: &str = env!("GIT_TAG");

/// The server backend
#[derive(Debug)]
struct Backend {
    /// The LSP client
    client: Arc<Client>,
    /// The workspace
    workspace: Arc<RwLock<Workspace>>
}

impl Backend {
    /// Create a new backend
    fn new(client: Client) -> Backend {
        Backend {
            client: Arc::new(client),
            workspace: Arc::new(RwLock::new(Workspace::default()))
        }
    }

    /// Execute the background work
    async fn worker(workspace: Arc<RwLock<Workspace>>, client: Arc<Client>) {
        info!("worker, starting ...");
        let mut workspace = workspace.write().await;
        workspace.lint();
        info!("worker, sending ...");
        join_all(workspace.documents.iter().map(|(uri, doc)| {
            client.publish_diagnostics(uri.clone(), doc.diagnostics.clone(), doc.version)
        }))
        .await;
        info!("worker, done!");
    }

    /// Execute the background work
    fn execute(&self) {
        info!("calling worker ...");
        tokio::spawn(Backend::worker(self.workspace.clone(), self.client.clone()));
    }
}

#[tower_lsp::async_trait]
impl LanguageServer for Backend {
    async fn initialize(&self, params: InitializeParams) -> Result<InitializeResult> {
        info!("initialize");
        let mut workspace = self.workspace.write().await;
        if let Some(root) = params.root_uri {
            if workspace.scan_workspace(root).is_err() {
                return Err(Error::internal_error());
            }
        }
        self.execute();

        Ok(InitializeResult {
            capabilities: ServerCapabilities {
                text_document_sync: Some(TextDocumentSyncCapability::Kind(
                    TextDocumentSyncKind::Full
                )),
                workspace: Some(WorkspaceServerCapabilities {
                    workspace_folders: Some(WorkspaceFoldersServerCapabilities {
                        supported: Some(true),
                        change_notifications: Some(OneOf::Left(true))
                    }),
                    file_operations: None
                }),
                execute_command_provider: Some(ExecuteCommandOptions {
                    commands: vec![String::from("test")],
                    work_done_progress_options: WorkDoneProgressOptions {
                        work_done_progress: Some(false)
                    }
                }),
                ..ServerCapabilities::default()
            },
            server_info: Some(ServerInfo {
                name: String::from(CRATE_NAME),
                version: Some(String::from(CRATE_VERSION))
            })
        })
    }

    async fn shutdown(&self) -> Result<()> {
        info!("shutdown");
        Ok(())
    }

    async fn did_change_watched_files(&self, params: DidChangeWatchedFilesParams) {
        info!("did_change_watched_files");
        let mut workspace = self.workspace.write().await;
        if workspace.on_file_events(&params.changes).is_err() {
            // do nothing
        }
        self.execute();
    }

    async fn did_change(&self, params: DidChangeTextDocumentParams) {
        info!("did_change");
        let mut workspace = self.workspace.write().await;
        workspace.on_file_changes(params);
        self.execute();
    }

    async fn execute_command(
        &self,
        params: ExecuteCommandParams
    ) -> Result<Option<serde_json::Value>> {
        info!("execute_command");
        let workspace = self.workspace.read().await;
        match &params.command[..] {
            "test" => {
                if params.arguments.len() != 2 {
                    Err(Error::invalid_params("Expected exactly 2 parameters"))
                } else {
                    match (&params.arguments[0], &params.arguments[1]) {
                        (
                            serde_json::Value::String(ref grammar),
                            serde_json::Value::String(ref input)
                        ) => workspace.test(grammar, input),
                        _ => Err(Error::invalid_params("Expected exactly 2 parameters"))
                    }
                }
            }
            _ => Err(Error::method_not_found())
        }
    }
}

fn setup_logger() -> std::result::Result<(), fern::InitError> {
    fern::Dispatch::new()
        .format(|out, message, record| out.finish(format_args!("[{}] {}", record.level(), message)))
        .level(log::LevelFilter::Debug)
        .chain(fern::log_file("output.log")?)
        .apply()?;
    Ok(())
}

#[tokio::main]
async fn main() {
    std::panic::set_hook(Box::new(|info| {
        let location = match info.location() {
            Some(location) => format!("{}:{}", location.file(), location.line()),
            None => String::from("unknown")
        };
        let message = match info.payload().downcast_ref::<String>() {
            Some(message) => message.to_owned(),
            None => String::from("no message")
        };
        error!("Panic: {} : {}", location, message);
    }));
    setup_logger().unwrap();

    let stdin = tokio::io::stdin();
    let stdout = tokio::io::stdout();

    let (service, messages) = LspService::new(Backend::new);
    Server::new(stdin, stdout)
        .interleave(messages)
        .serve(service)
        .await;
}
