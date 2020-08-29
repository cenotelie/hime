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

extern crate hime_redist;
extern crate hime_sdk;
extern crate tokio;
extern crate tower_lsp;

pub mod workspace;

use std::sync::Arc;
use tokio::sync::RwLock;
use tower_lsp::jsonrpc::{Error, Result};
use tower_lsp::lsp_types::*;
use tower_lsp::{Client, LanguageServer, LspService, Server};
use workspace::Workspace;

/// The name of this program
pub const CRATE_NAME: &str = env!("CARGO_PKG_NAME");
/// The version of this program
pub const CRATE_VERSION: &str = env!("CARGO_PKG_VERSION");

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
            workspace: Arc::new(RwLock::new(Workspace::default())) // thread: Arc::new(RwLock::new(None)),
                                                                   // comm: Arc::new((Mutex::new(WorkerCommand::Wait), Condvar::new()))
        }
    }

    /// Execute the background work
    async fn worker(workspace: Arc<RwLock<Workspace>>, client: Arc<Client>) {
        let mut workspace = workspace.write().await;
        workspace.lint();
        for (uri, doc) in workspace.documents.iter() {
            if !doc.diagnostics.is_empty() {
                client
                    .publish_diagnostics(uri.clone(), doc.diagnostics.clone(), doc.version)
                    .await;
            }
        }
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
                    TextDocumentSyncKind::Full
                )),
                workspace: Some(WorkspaceCapability {
                    workspace_folders: Some(WorkspaceFolderCapability {
                        supported: Some(true),
                        change_notifications: Some(
                            WorkspaceFolderCapabilityChangeNotifications::Bool(true)
                        )
                    })
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
}

#[tokio::main]
async fn main() {
    let stdin = tokio::io::stdin();
    let stdout = tokio::io::stdout();

    let (service, messages) = LspService::new(|client| Backend::new(client));
    Server::new(stdin, stdout)
        .interleave(messages)
        .serve(service)
        .await;
}
