/*******************************************************************************
 * Copyright (c) 2018 Association Cénotélie (cenotelie.fr)
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

//! Module for the definition of Hime grammars

pub mod symbols;
pub mod rules;

use std::collections::HashMap;

use grammar::rules::Rule;
use grammar::symbols::Action;
use grammar::symbols::Symbol;
use grammar::symbols::SymbolReference;
use grammar::symbols::Terminal;
use grammar::symbols::Variable;
use grammar::symbols::Virtual;

use automata::nfa::NFA;

use super::Report;

/// The identifier of a grammar symbol
pub type SymbolId = usize;

/// The prefix for the generated terminal names
pub const PREFIX_GENERATED_TERMINAL: &'static str = "__T";

/// The prefix for the generated variable names
pub const PREFIX_GENERATED_VARIABLE: &'static str = "__V";

/// The name of the generated axiom variable
pub const GENERATED_AXIOM: &'static str = "__VAxiom";

/// Name of the grammar option specifying the grammar's axiom variable
pub const OPTION_AXIOM: &'static str = "Axiom";

/// Name of the grammar option specifying the grammar's separator terminal
pub const OPTION_SEPARATOR: &'static str = "Separator";

/// The output path for compilation artifacts
pub const OPTION_OUTPUT_PATH: &'static str = "OutputPath";

/// The compilation mode to use, defaults to Source
pub const OPTION_COMPILATION_MODE: &'static str = "CompilationMode";

/// The parser type to generate, defaults to LALR1
pub const OPTION_PARSER_TYPE: &'static str = "ParserType";

/// The runtime to target, defaults to Net
pub const OPTION_RUNTIME: &'static str = "Runtime";

/// The namespace to use for the generated code
pub const OPTION_NAMESPACE: &'static str = "Namespace";

/// The access mode for the generated code, defaults to Internal
pub const OPTION_ACCESS_MODIFIER: &'static str = "AccessModifier";

/// The name of the default lexical context
pub const DEFAULT_CONTEXT_NAME: &'static str = "__default";

/// Represents a grammar
pub struct Grammar {
    /// The grammar's name
    name: String,
    /// The next unique symbol identifier for this grammar
    next_id: usize,
    /// The grammar's options
    options: Vec<(String, String)>,
    /// The lexical contexts defined in this grammar
    contexts: Vec<String>,
    /// The fragments of terminals (used in the definition of complete terminals)
    fragments: Vec<Terminal>,
    /// The grammar's terminals
    terminals: Vec<Terminal>,
    /// The grammar's variables
    variables: Vec<Variable>,
    /// The grammar's virtual symbols
    virtuals: Vec<Virtual>,
    /// The grammar's action symbols
    actions: Vec<Action>,
    /// The grammar's rules
    rules: HashMap<SymbolId, Vec<Rule>>
}

impl Grammar {
    /// Creates a new empty grammar
    pub fn new(name: String) -> Grammar {
        let mut result = Grammar {
            name,
            next_id: 3,
            options: Vec::<(String, String)>::new(),
            contexts: Vec::<String>::new(),
            fragments: Vec::<Terminal>::new(),
            terminals: Vec::<Terminal>::new(),
            variables: Vec::<Variable>::new(),
            virtuals: Vec::<Virtual>::new(),
            actions: Vec::<Action>::new(),
            rules: HashMap::<SymbolId, Vec<Rule>>::new()
        };
        result.contexts.push(DEFAULT_CONTEXT_NAME.to_owned());
        result
    }

    /// Gets the grammar's name
    pub fn name(&self) -> &str {
        &self.name
    }

    /// Adds an option to the grammar
    pub fn add_option(&mut self, name: &str, value: &str) {
        let maybe = self.options
            .iter()
            .enumerate()
            .find(|&(_i, &(ref n, ref _v))| n.eq(name))
            .map(|(i, &(ref _n, ref _v))| i);
        match maybe {
            None => {
                self.options.push((name.to_owned(), value.to_owned()));
            }
            Some(index) => {
                self.options[index] = (name.to_owned(), value.to_owned());
            }
        }
    }

    /// Gets the value of the given option
    pub fn get_option(&self, name: &str) -> Option<&str> {
        self.options
            .iter()
            .find(|&&(ref n, ref _v)| n.eq(name))
            .map(|&(ref _n, ref v)| v.as_ref())
    }

    /// Gets the symbol with the given name in this grammar
    pub fn get_symbol(&self, name: &str) -> Option<SymbolReference> {
        let maybe = self.terminals
            .iter()
            .find(|ref symbol| symbol.name().eq(name));
        if maybe.is_some() {
            return Some(SymbolReference::Terminal(maybe.unwrap().id()));
        }
        let maybe = self.variables
            .iter()
            .find(|ref symbol| symbol.name().eq(name));
        if maybe.is_some() {
            return Some(SymbolReference::Variable(maybe.unwrap().id()));
        }
        let maybe = self.virtuals
            .iter()
            .find(|ref symbol| symbol.name().eq(name));
        if maybe.is_some() {
            return Some(SymbolReference::Virtual(maybe.unwrap().id()));
        }
        let maybe = self.actions
            .iter()
            .find(|ref symbol| symbol.name().eq(name));
        maybe.map(|ref symbol| SymbolReference::Action(symbol.id()))
    }

    /// Resolves the specified lexical context name for this grammar
    pub fn resolve_context(&mut self, context: &str) -> usize {
        let maybe = self.contexts
            .iter()
            .enumerate()
            .find(|&(_i, ref c)| c.eq(&context))
            .map(|(i, ref _c)| i);
        match maybe {
            Some(index) => index,
            None => {
                let index = self.contexts.len();
                self.contexts.push(context.to_owned());
                index
            }
        }
    }

    /// Gets the name of the context with the specified identifier
    pub fn get_context_name(&self, context: usize) -> &str {
        &self.contexts[context]
    }

    /// Adds the given fragment to this grammar
    pub fn add_fragment(&mut self, name: &str, nfa: NFA) -> SymbolReference {
        let id = self.next_id;
        self.next_id += 1;
        self.fragments
            .push(Terminal::new(id, name.to_owned(), name.to_owned(), 0, nfa));
        SymbolReference::Terminal(id)
    }

    /// Gets the fragment with the given name
    pub fn get_fragment(&self, name: &str) -> Option<&Terminal> {
        self.fragments
            .iter()
            .find(|ref terminal| terminal.name().eq(name))
    }

    /// Adds the given anonymous terminal to this grammar
    pub fn add_terminal_anonymous(&mut self, value: &str, nfa: NFA) -> SymbolReference {
        let id = self.next_id;
        self.next_id += 1;
        let name = format!("{:}{:02$X}", PREFIX_GENERATED_TERMINAL, id, 4);
        self.terminals
            .push(Terminal::new(id, name, value.to_owned(), 0, nfa));
        SymbolReference::Terminal(id)
    }

    /// Adds the given named terminal to this grammar
    pub fn add_terminal_named(&mut self, name: &str, nfa: NFA, context: usize) -> SymbolReference {
        let id = self.next_id;
        self.next_id += 1;
        self.terminals.push(Terminal::new(
            id,
            name.to_owned(),
            name.to_owned(),
            context,
            nfa
        ));
        SymbolReference::Terminal(id)
    }

    /// Gets the terminal with the given name
    pub fn get_terminal_by_name(&self, name: &str) -> Option<&Terminal> {
        self.terminals
            .iter()
            .find(|ref terminal| terminal.name().eq(name))
    }

    /// Gets the terminal with the given name
    pub fn get_terminal_by_value(&self, value: &str) -> Option<&Terminal> {
        self.terminals
            .iter()
            .find(|ref terminal| terminal.value().eq(value))
    }

    /// Generates a new variable
    pub fn generate_variable(&mut self) -> SymbolReference {
        let id = self.next_id;
        self.next_id += 1;
        let name = format!("{:}{:02$X}", PREFIX_GENERATED_VARIABLE, id, 4);
        self.variables.push(Variable::new(id, name));
        SymbolReference::Variable(id)
    }

    /// Adds a variable with the given name to this grammar
    pub fn resolve_variable(&mut self, name: &str) -> SymbolReference {
        let maybe = self.variables
            .iter()
            .find(|ref symbol| symbol.name().eq(name))
            .map(|variable| variable.id());
        if maybe.is_some() {
            return SymbolReference::Variable(maybe.unwrap());
        }
        let id = self.next_id;
        self.next_id += 1;
        self.variables.push(Variable::new(id, name.to_owned()));
        SymbolReference::Variable(id)
    }

    /// Gets the variable with the given name
    pub fn get_variable(&self, name: &str) -> Option<&Variable> {
        self.variables
            .iter()
            .find(|ref variable| variable.name().eq(name))
    }

    /// Adds a virtual symbol with the given name to this grammar
    pub fn resolve_virtual(&mut self, name: &str) -> SymbolReference {
        let maybe = self.virtuals
            .iter()
            .find(|ref symbol| symbol.name().eq(name))
            .map(|symbol| symbol.id());
        if maybe.is_some() {
            return SymbolReference::Virtual(maybe.unwrap());
        }
        let id = self.next_id;
        self.next_id += 1;
        self.virtuals.push(Virtual::new(id, name.to_owned()));
        SymbolReference::Virtual(id)
    }

    /// Gets the virtual symbol with the given name
    pub fn get_virtual(&self, name: &str) -> Option<&Virtual> {
        self.virtuals
            .iter()
            .find(|ref symbol| symbol.name().eq(name))
    }

    /// Adds an action symbol with the given name to this grammar
    pub fn resolve_action(&mut self, name: &str) -> SymbolReference {
        let maybe = self.actions
            .iter()
            .find(|ref symbol| symbol.name().eq(name))
            .map(|symbol| symbol.id());
        if maybe.is_some() {
            return SymbolReference::Action(maybe.unwrap());
        }
        let id = self.next_id;
        self.next_id += 1;
        self.actions.push(Action::new(id, name.to_owned()));
        SymbolReference::Action(id)
    }

    /// Gets the action symbol with the given name
    pub fn get_action(&self, name: &str) -> Option<&Action> {
        self.actions
            .iter()
            .find(|ref symbol| symbol.name().eq(name))
    }

    /// Gets the grammar rules
    pub fn get_rules(&self) -> Vec<&Rule> {
        let mut result = Vec::<&Rule>::new();
        self.rules
            .iter()
            .for_each(|(ref _id, ref rules)| rules.iter().for_each(|ref rule| result.push(rule)));
        result
    }

    /// Inherit from the given parent
    pub fn inherit_from(&mut self, parent: &Grammar, report: &mut Report) {
        let keep_sid = self.next_id == 3;
        let mut id_map = HashMap::<SymbolId, SymbolId>::new();
        self.inherit_options(parent);
        self.inherit_fragments(parent, report, keep_sid, &mut id_map);
        self.inherit_terminals(parent, report, keep_sid, &mut id_map);
        self.inherit_variables(parent, keep_sid, &mut id_map);
        self.inherit_virtuals(parent, keep_sid, &mut id_map);
        self.inherit_actions(parent, keep_sid, &mut id_map);
        self.inherit_rules(parent, &id_map);

        if keep_sid {
            self.next_id = self.fragments
                .iter()
                .fold(self.next_id, |next_id, ref symbol| next_id.max(symbol.id()));
            self.next_id = self.terminals
                .iter()
                .fold(self.next_id, |next_id, ref symbol| next_id.max(symbol.id()));
            self.next_id = self.variables
                .iter()
                .fold(self.next_id, |next_id, ref symbol| next_id.max(symbol.id()));
            self.next_id = self.virtuals
                .iter()
                .fold(self.next_id, |next_id, ref symbol| next_id.max(symbol.id()));
            self.next_id = self.actions
                .iter()
                .fold(self.next_id, |next_id, ref symbol| next_id.max(symbol.id()));
            self.next_id += 1;
        }
    }

    /// Inherits the options from the parent grammar
    fn inherit_options(&mut self, parent: &Grammar) {
        parent
            .options
            .iter()
            .for_each(|&(ref name, ref value)| self.add_option(&name, &value));
    }

    /// Inherits the fragments from the parent grammar
    fn inherit_fragments(
        &mut self,
        parent: &Grammar,
        report: &mut Report,
        keep_sid: bool,
        id_map: &mut HashMap<SymbolId, SymbolId>
    ) {
        for symbol in parent.fragments.iter() {
            let contains = self.fragments
                .iter()
                .find(|ref symbol| symbol.name().eq(symbol.name()))
                .is_some();
            if contains {
                report.error(format!(
                    "In grammar {:}, the terminal fragment {:} is redefined when imported from {:}",
                    self.name,
                    symbol.name(),
                    parent.name
                ));
            } else if keep_sid {
                self.fragments.push(symbol.clone());
                id_map.insert(symbol.id(), symbol.id());
            } else {
                self.fragments.push(symbol.clone_with_id(self.next_id, 0));
                id_map.insert(symbol.id(), self.next_id);
                self.next_id += 1;
            }
        }
    }

    /// Inherits the terminals from the parent grammar
    fn inherit_terminals(
        &mut self,
        parent: &Grammar,
        report: &mut Report,
        keep_sid: bool,
        id_map: &mut HashMap<SymbolId, SymbolId>
    ) {
        for symbol in parent.terminals.iter() {
            let contains = self.terminals
                .iter()
                .find(|ref symbol| symbol.name().eq(symbol.name()))
                .is_some();
            if contains {
                report.error(format!(
                    "In grammar {:}, the named terminal {:} is redefined when imported from {:}",
                    self.name,
                    symbol.name(),
                    parent.name
                ));
            } else if keep_sid {
                let target_context = self.resolve_context(&parent.contexts[symbol.context()]);
                self.terminals
                    .push(symbol.clone_with_id(symbol.id(), target_context));
                id_map.insert(symbol.id(), symbol.id());
            } else {
                let target_context = self.resolve_context(&parent.contexts[symbol.context()]);
                self.terminals
                    .push(symbol.clone_with_id(self.next_id, target_context));
                id_map.insert(symbol.id(), self.next_id);
                self.next_id += 1;
            }
        }
    }

    /// Inherits the variables from the parent grammar
    fn inherit_variables(
        &mut self,
        parent: &Grammar,
        keep_sid: bool,
        id_map: &mut HashMap<SymbolId, SymbolId>
    ) {
        for symbol in parent.variables.iter() {
            let maybe_id = self.variables
                .iter()
                .find(|ref symbol| symbol.name().eq(symbol.name()))
                .map(|ref variable| variable.id());
            if maybe_id.is_some() {
                id_map.insert(symbol.id(), maybe_id.unwrap());
            } else if keep_sid {
                self.variables.push(symbol.clone());
                id_map.insert(symbol.id(), symbol.id());
            } else {
                self.variables.push(symbol.clone_with_id(self.next_id));
                id_map.insert(symbol.id(), self.next_id);
                self.next_id += 1;
            }
        }
    }

    /// Inherits the virtuals from the parent grammar
    fn inherit_virtuals(
        &mut self,
        parent: &Grammar,
        keep_sid: bool,
        id_map: &mut HashMap<SymbolId, SymbolId>
    ) {
        for symbol in parent.virtuals.iter() {
            let maybe_id = self.virtuals
                .iter()
                .find(|ref symbol| symbol.name().eq(symbol.name()))
                .map(|ref variable| variable.id());
            if maybe_id.is_some() {
                id_map.insert(symbol.id(), maybe_id.unwrap());
            } else if keep_sid {
                self.virtuals.push(symbol.clone());
                id_map.insert(symbol.id(), symbol.id());
            } else {
                self.virtuals.push(symbol.clone_with_id(self.next_id));
                id_map.insert(symbol.id(), self.next_id);
                self.next_id += 1;
            }
        }
    }

    /// Inherits the actions from the parent grammar
    fn inherit_actions(
        &mut self,
        parent: &Grammar,
        keep_sid: bool,
        id_map: &mut HashMap<SymbolId, SymbolId>
    ) {
        for symbol in parent.actions.iter() {
            let maybe_id = self.actions
                .iter()
                .find(|ref symbol| symbol.name().eq(symbol.name()))
                .map(|ref variable| variable.id());
            if maybe_id.is_some() {
                id_map.insert(symbol.id(), maybe_id.unwrap());
            } else if keep_sid {
                self.actions.push(symbol.clone());
                id_map.insert(symbol.id(), symbol.id());
            } else {
                self.actions.push(symbol.clone_with_id(self.next_id));
                id_map.insert(symbol.id(), self.next_id);
                self.next_id += 1;
            }
        }
    }

    /// Inherits the grammar rules from the parent grammar
    fn inherit_rules(&mut self, parent: &Grammar, id_map: &HashMap<SymbolId, SymbolId>) {
        for (variable_id, rules) in parent.rules.iter() {
            let target = *id_map.get(variable_id).unwrap();
            if !self.rules.contains_key(&target) {
                self.rules.insert(target, Vec::<Rule>::new());
            }
            for rule in rules.iter() {
                let target_context = self.resolve_context(&parent.contexts[rule.context()]);
                self.rules
                    .get_mut(&target)
                    .unwrap()
                    .push(rule.clone_with_ids(id_map, target_context));
            }
        }
    }
}
