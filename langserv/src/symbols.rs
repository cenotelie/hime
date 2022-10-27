use std::collections::HashMap;

use hime_sdk::grammars::{Grammar, SymbolRef};
use hime_sdk::InputReference;

/*******************************************************************************
 * Copyright (c) 2021 Association Cénotélie (cenotelie.fr)
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

/// A symbol in the registry
#[derive(Debug, Clone)]
pub struct SymbolRegistryElement {
    /// The index of the defining grammar
    pub grammar_index: usize,
    /// Reference to the symbol
    pub symbol_ref: SymbolRef,
    /// The input reference for its definition
    pub definitions: Vec<InputReference>,
    /// All the usage references
    pub references: Vec<InputReference>
}

impl SymbolRegistryElement {
    /// Gets whether this symbol is at a location
    pub fn is_at_location(&self, location: &InputReference) -> bool {
        self.definitions
            .iter()
            .any(|input_ref| input_ref.overlaps_with(location))
            || self
                .references
                .iter()
                .any(|input_ref| input_ref.overlaps_with(location))
    }
}

/// A registry of symbols and their references
#[derive(Debug, Clone, Default)]
pub struct SymbolRegistry {
    /// The symbols for each grammar
    pub grammars: Vec<HashMap<SymbolRef, SymbolRegistryElement>>
}

impl SymbolRegistry {
    /// Initializes a registry from grammars
    pub fn from(grammars: &[Grammar]) -> SymbolRegistry {
        SymbolRegistry {
            grammars: grammars
                .iter()
                .enumerate()
                .map(|(grammar_index, grammar)| {
                    let mut map = HashMap::new();
                    for terminal in grammar.terminals.iter() {
                        map.insert(
                            SymbolRef::Terminal(terminal.id),
                            SymbolRegistryElement {
                                grammar_index,
                                symbol_ref: SymbolRef::Terminal(terminal.id),
                                definitions: vec![terminal.input_ref],
                                references: terminal
                                    .terminal_references
                                    .iter()
                                    .map(|term_ref| term_ref.input_ref)
                                    .collect()
                            }
                        );
                    }
                    for variable in grammar.variables.iter() {
                        map.insert(
                            SymbolRef::Variable(variable.id),
                            SymbolRegistryElement {
                                grammar_index,
                                symbol_ref: SymbolRef::Variable(variable.id),
                                definitions: variable
                                    .rules
                                    .iter()
                                    .map(|rule| rule.head_input_ref)
                                    .collect(),
                                references: Vec::new()
                            }
                        );
                    }
                    for symbol in grammar.virtuals.iter() {
                        map.insert(
                            SymbolRef::Virtual(symbol.id),
                            SymbolRegistryElement {
                                grammar_index,
                                symbol_ref: SymbolRef::Virtual(symbol.id),
                                definitions: Vec::new(),
                                references: Vec::new()
                            }
                        );
                    }
                    for symbol in grammar.actions.iter() {
                        map.insert(
                            SymbolRef::Action(symbol.id),
                            SymbolRegistryElement {
                                grammar_index,
                                symbol_ref: SymbolRef::Action(symbol.id),
                                definitions: Vec::new(),
                                references: Vec::new()
                            }
                        );
                    }
                    // retrieve all references in syntactic rules
                    for variable in grammar.variables.iter() {
                        for rule in variable.rules.iter() {
                            for element in rule.body.elements.iter() {
                                if let Some(input_ref) = element.input_ref {
                                    if let Some(entry) = map.get_mut(&element.symbol) {
                                        entry.references.push(input_ref);
                                    }
                                }
                            }
                        }
                    }
                    map
                })
                .collect()
        }
    }
}
