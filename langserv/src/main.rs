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

pub mod symbols;
pub mod workspace;

use std::sync::Arc;

use clap::{Arg, Command};
use futures::future::join_all;
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
        let mut workspace = workspace.write().await;
        workspace.lint();
        join_all(workspace.documents.iter().map(|(uri, doc)| {
            client.publish_diagnostics(uri.clone(), doc.diagnostics.clone(), doc.version)
        }))
        .await;
    }

    /// Execute the background work
    fn execute(&self) {
        tokio::spawn(Backend::worker(self.workspace.clone(), self.client.clone()));
    }
}

#[tower_lsp::async_trait]
impl LanguageServer for Backend {
    async fn initialize(&self, params: InitializeParams) -> Result<InitializeResult> {
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
                    TextDocumentSyncKind::FULL
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
                hover_provider: Some(HoverProviderCapability::Simple(true)),
                ..ServerCapabilities::default()
            },
            server_info: Some(ServerInfo {
                name: String::from(CRATE_NAME),
                version: Some(String::from(CRATE_VERSION))
            })
        })
    }

    async fn shutdown(&self) -> Result<()> {
        Ok(())
    }

    async fn did_change_watched_files(&self, params: DidChangeWatchedFilesParams) {
        let mut workspace = self.workspace.write().await;
        if workspace.on_file_events(&params.changes).is_err() {
            // do nothing
        }
        self.execute();
    }

    async fn did_change(&self, params: DidChangeTextDocumentParams) {
        let mut workspace = self.workspace.write().await;
        workspace.on_file_changes(params);
        self.execute();
    }

    async fn symbol(
        &self,
        params: WorkspaceSymbolParams
    ) -> Result<Option<Vec<SymbolInformation>>> {
        let workspace = self.workspace.read().await;
        let data = workspace.lookup_symbols(&params.query);
        if data.is_empty() {
            Ok(None)
        } else {
            Ok(Some(data))
        }
    }

    async fn hover(&self, params: HoverParams) -> Result<Option<Hover>> {
        Ok(None)
    }

    async fn execute_command(
        &self,
        params: ExecuteCommandParams
    ) -> Result<Option<serde_json::Value>> {
        let workspace = self.workspace.read().await;
        match &params.command[..] {
            "test" => {
                if params.arguments.len() != 2 {
                    Err(Error::invalid_params("Expected exactly 2 parameters"))
                } else {
                    match (&params.arguments[0], &params.arguments[1]) {
                        (serde_json::Value::String(grammar), serde_json::Value::String(input)) => {
                            workspace.test(grammar, input)
                        }
                        _ => Err(Error::invalid_params("Expected exactly 2 parameters"))
                    }
                }
            }
            _ => Err(Error::method_not_found())
        }
    }
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
        eprintln!("Panic: {} : {}", location, message);
    }));

    let matches = Command::new("Hime Language Server")
        .version(
            format!(
                "{} {} tag={} hash={}",
                CRATE_NAME, CRATE_VERSION, GIT_TAG, GIT_HASH
            )
            .as_str()
        )
        .author("Association Cénotélie <contact@cenotelie.fr>")
        .about("Language server for Hime gramamrs")
        .arg(
            Arg::new("tcp")
                .long("tcp")
                .help("Use a tcp stream to communicate with clients")
                .takes_value(false)
                .required(false)
        )
        .arg(
            Arg::new("address")
                .value_name("ADDRESS")
                .long("address")
                .short('a')
                .help("The address to listen on, if using a TCP stream")
                .takes_value(true)
                .required(false)
        )
        .arg(
            Arg::new("port")
                .long("port")
                .short('p')
                .help("The TCP port to listen to, if using a TCP stream")
                .takes_value(true)
                .required(false)
        )
        .subcommand(Command::new("version").about("Display the version string"))
        .get_matches();

    match matches.subcommand() {
        Some(("version", _)) => {
            println!(
                "{} {} tag={} hash={}",
                CRATE_NAME, CRATE_VERSION, GIT_TAG, GIT_HASH
            )
        }
        _ => {
            if matches.is_present("tcp") {
                let address = matches.value_of("address").unwrap_or("127.0.0.1");
                let port = matches
                    .value_of("port")
                    .map(|v| v.parse::<u16>())
                    .transpose()
                    .unwrap_or_default()
                    .unwrap_or(9257);
                println!("Listening on {}:{}", address, port);
                let listener = tokio::net::TcpListener::bind(format!("{}:{}", address, port))
                    .await
                    .unwrap();
                let (stream, _) = listener.accept().await.unwrap();
                let (read, write) = tokio::io::split(stream);
                let (service, messages) = LspService::new(Backend::new);
                Server::new(read, write)
                    .interleave(messages)
                    .serve(service)
                    .await;
            } else {
                let stdin = tokio::io::stdin();
                let stdout = tokio::io::stdout();
                let (service, messages) = LspService::new(Backend::new);
                Server::new(stdin, stdout)
                    .interleave(messages)
                    .serve(service)
                    .await;
            }
        }
    }
}
