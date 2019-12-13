/*******************************************************************************
 * Copyright (c) 2019 Association Cénotélie (cenotelie.fr)
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

//! Library for grammars

use crate::automata::fa::NFA;
use crate::automata::lr::SymbolRef;
use hime_redist::parsers::{TreeAction, TREE_ACTION_NONE};
use std::cmp::Ordering;
use std::collections::HashMap;
use std::sync::atomic::{AtomicUsize, Ordering as AtomicOrdering};

/// Represents a symbol in a grammar
pub trait Symbol {
    /// Gets the unique indentifier (within a grammar) of this symbol
    fn get_id(&self) -> usize;

    /// Gets the name of this symbol
    fn get_name(&self) -> &str;
}

/// Represents a terminal symbol in a grammar
#[derive(Debug, Clone)]
pub struct Terminal {
    /// The unique indentifier (within a grammar) of this symbol
    pub id: usize,
    /// The name of this symbol
    pub name: String,
    /// The inline value of this terminal
    pub value: String,
    /// The NFA that is used to match this terminal
    pub nfa: NFA,
    /// The context of this terminal
    pub context: usize,
    /// Whether the terminal is a fragment
    pub is_fragment: bool
}

impl Terminal {
    /// Gets the priority of this terminal
    pub fn priority(&self) -> usize {
        self.id
    }
}

impl Symbol for Terminal {
    /// Gets the unique indentifier (within a grammar) of this symbol
    fn get_id(&self) -> usize {
        self.id
    }

    /// Gets the name of this symbol
    fn get_name(&self) -> &str {
        &self.name
    }
}

impl PartialEq for Terminal {
    fn eq(&self, other: &Self) -> bool {
        self.id == other.id
    }
}

impl Eq for Terminal {}

impl Ord for Terminal {
    fn cmp(&self, other: &Terminal) -> Ordering {
        self.id.cmp(&other.id)
    }
}

impl PartialOrd for Terminal {
    fn partial_cmp(&self, other: &Terminal) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

/// Represents a reference to a terminal-like
#[derive(Debug, Copy, Clone, Eq, PartialEq)]
pub enum TerminalRef {
    /// Represents a fake terminal, used as a marker by LR-related algorithms
    Dummy,
    /// Represents the epsilon symbol in a grammar, i.e. a terminal with an empty value
    Epsilon,
    /// Represents the dollar symbol in a grammar, i.e. the marker of end of input
    Dollar,
    /// Represents the absence of terminal, used as a marker by LR-related algorithms
    NullTerminal,
    /// A terminal in a grammar
    Terminal(usize)
}

impl TerminalRef {
    /// Gets the terminal priority
    pub fn priority(self) -> usize {
        match self {
            TerminalRef::Dummy => 0,
            TerminalRef::Epsilon => 1,
            TerminalRef::Dollar => 2,
            TerminalRef::NullTerminal => 0,
            TerminalRef::Terminal(id) => id
        }
    }
}

impl Ord for TerminalRef {
    fn cmp(&self, other: &TerminalRef) -> Ordering {
        self.priority().cmp(&other.priority())
    }
}

impl PartialOrd for TerminalRef {
    fn partial_cmp(&self, other: &TerminalRef) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

/// Represents a set of unique terminals (sorted by ID)
#[derive(Debug, Clone, Default)]
pub struct TerminalSet {
    /// The backing content
    pub content: Vec<TerminalRef>
}

impl TerminalSet {
    /// Gets the number of states in this automaton
    pub fn len(&self) -> usize {
        self.content.len()
    }

    /// Gets whether the NFA has no state
    pub fn is_empty(&self) -> bool {
        self.content.is_empty()
    }

    /// Adds a new terminal
    fn do_add(&mut self, item: TerminalRef) -> bool {
        if !self.content.contains(&item) {
            self.content.push(item);
            true
        } else {
            false
        }
    }

    /// Adds a new terminal
    pub fn add(&mut self, item: TerminalRef) -> bool {
        let modified = self.do_add(item);
        self.content.sort();
        modified
    }

    /// Adds new terminals
    pub fn add_others(&mut self, others: &TerminalSet) -> bool {
        let mut modified = false;
        for item in others.content.iter() {
            modified |= self.do_add(*item);
        }
        self.content.sort();
        modified
    }

    /// Removes all items from this collection
    pub fn clear(&mut self) {
        self.content.clear();
    }
}

/// Represents a virtual symbol in a grammar
#[derive(Debug, Clone)]
pub struct Virtual {
    /// The unique indentifier (within a grammar) of this symbol
    pub id: usize,
    /// The name of this symbol
    pub name: String
}

impl Virtual {
    /// Creates a new variable
    pub fn new(id: usize, name: String) -> Virtual {
        Virtual { id, name }
    }
}

impl Symbol for Virtual {
    /// Gets the unique indentifier (within a grammar) of this symbol
    fn get_id(&self) -> usize {
        self.id
    }

    /// Gets the name of this symbol
    fn get_name(&self) -> &str {
        &self.name
    }
}

impl PartialEq for Virtual {
    fn eq(&self, other: &Self) -> bool {
        self.id == other.id
    }
}

impl Eq for Virtual {}

/// Represents a symbol for a semantic action in a grammar
#[derive(Debug, Clone)]
pub struct Action {
    /// The unique indentifier (within a grammar) of this symbol
    pub id: usize,
    /// The name of this symbol
    pub name: String
}

impl Action {
    /// Creates a new variable
    pub fn new(id: usize, name: String) -> Action {
        Action { id, name }
    }
}

impl Symbol for Action {
    /// Gets the unique indentifier (within a grammar) of this symbol
    fn get_id(&self) -> usize {
        self.id
    }

    /// Gets the name of this symbol
    fn get_name(&self) -> &str {
        &self.name
    }
}

impl PartialEq for Action {
    fn eq(&self, other: &Self) -> bool {
        self.id == other.id
    }
}

impl Eq for Action {}

/// Represents a variable in a grammar
#[derive(Debug, Clone)]
pub struct Variable {
    /// The unique indentifier (within a grammar) of this symbol
    pub id: usize,
    /// The name of this symbol
    pub name: String,
    /// The rules for this variable
    pub rules: Vec<Rule>,
    /// The FIRSTS set for this variable
    pub firsts: TerminalSet,
    /// The FOLLOWERS set for this variable
    pub followers: TerminalSet
}

impl Variable {
    /// Creates a new variable
    pub fn new(id: usize, name: String) -> Variable {
        Variable {
            id,
            name,
            rules: Vec::new(),
            firsts: TerminalSet::default(),
            followers: TerminalSet::default()
        }
    }

    /// Adds the given rule for this variable as a unique element
    pub fn add_rule(&mut self, rule: Rule) {
        if !self.rules.contains(&rule) {
            self.rules.push(rule);
        }
    }

    /// Compute rule choices for all rules for this variable
    pub fn compute_choices(&mut self) {
        for rule in self.rules.iter_mut() {
            rule.body.compute_choices();
        }
    }

    /// Computes the FIRSTS set for this variable
    pub fn compute_firsts(&mut self, firsts_for_var: &mut HashMap<usize, TerminalSet>) -> bool {
        let mut modified = false;
        for rule in self.rules.iter_mut() {
            modified |= self.firsts.add_others(&rule.body.firsts);
            modified |= rule.body.compute_firsts(firsts_for_var);
        }
        modified
    }

    /// Computes the initial FOLLOWERS sets
    pub fn compute_initial_follower(&self, followers: &mut HashMap<usize, TerminalSet>) {
        for rule in self.rules.iter() {
            rule.body.compute_initial_follower(followers);
        }
    }

    /// Propagate the followers
    pub fn propagate_followers(&self, followers: &mut HashMap<usize, TerminalSet>) -> bool {
        let mut modified = false;
        for rule in self.rules.iter() {
            modified |= rule.body.propagate_followers(rule.head, followers);
        }
        modified
    }
}

impl Symbol for Variable {
    /// Gets the unique indentifier (within a grammar) of this symbol
    fn get_id(&self) -> usize {
        self.id
    }

    /// Gets the name of this symbol
    fn get_name(&self) -> &str {
        &self.name
    }
}

impl PartialEq for Variable {
    fn eq(&self, other: &Self) -> bool {
        self.id == other.id
    }
}

impl Eq for Variable {}

/// Represents an element in the body of a grammar rule
#[derive(Debug, Copy, Clone, Eq, PartialEq)]
pub struct RuleBodyElement {
    /// The symbol of this element
    pub symbol: SymbolRef,
    /// The action applied on this element
    pub action: TreeAction
}

impl RuleBodyElement {
    /// Creates a new body element
    pub fn new(symbol: SymbolRef, action: TreeAction) -> RuleBodyElement {
        RuleBodyElement { symbol, action }
    }
}

/// Represents a choice in a rule, i.e. the remainder of a rule's body
#[derive(Debug, Clone, Default)]
pub struct RuleChoice {
    /// The elements in this body
    pub parts: Vec<RuleBodyElement>,
    /// The FIRSTS set of terminals
    pub firsts: TerminalSet
}

impl RuleChoice {
    /// Creates a new choice from a single symbol
    pub fn new(symbol: SymbolRef) -> RuleChoice {
        RuleChoice {
            parts: vec![RuleBodyElement::new(symbol, TREE_ACTION_NONE)],
            firsts: TerminalSet::default()
        }
    }

    /// Initializes this rule body from parts
    pub fn from_parts(parts: &[RuleBodyElement]) -> RuleBody {
        RuleBody {
            parts: parts.to_vec(),
            firsts: TerminalSet::default(),
            choices: Vec::new()
        }
    }

    /// Gets the length of the rule choice
    pub fn len(&self) -> usize {
        self.parts.len()
    }

    /// Gets wether the rule choice is empty
    pub fn is_empty(&self) -> bool {
        self.parts.is_empty()
    }

    /// Appends a single symbol to the choice
    pub fn append_symbol(&mut self, symbol: SymbolRef) {
        self.parts
            .push(RuleBodyElement::new(symbol, TREE_ACTION_NONE));
    }

    /// Appends the content of another choice to this one
    pub fn append_choice(&mut self, other: &RuleChoice) {
        for element in other.parts.iter() {
            self.parts.push(*element);
        }
    }

    /// Computes the FIRSTS set for this rule choice
    pub fn compute_firsts(
        &mut self,
        next: &TerminalSet,
        firsts_for_var: &mut HashMap<usize, TerminalSet>
    ) -> bool {
        // If the choice is empty : Add the ε to the Firsts and return
        if self.parts.is_empty() {
            return self.firsts.add(TerminalRef::Epsilon);
        }
        match self.parts[0].symbol {
            SymbolRef::Variable(id) => {
                let mut modified = false;
                if let Some(var_firsts) = firsts_for_var.get(&id) {
                    for first in var_firsts.content.iter() {
                        match *first {
                            TerminalRef::Epsilon => {
                                modified |= self.firsts.add_others(next);
                            }
                            _ => {
                                modified |= self.firsts.add(*first);
                            }
                        }
                    }
                }
                modified
            }
            SymbolRef::Terminal(id) => self.firsts.add(TerminalRef::Terminal(id)),
            SymbolRef::Dummy => self.firsts.add(TerminalRef::Dummy),
            SymbolRef::Epsilon => self.firsts.add(TerminalRef::Epsilon),
            SymbolRef::Dollar => self.firsts.add(TerminalRef::Dollar),
            SymbolRef::NullTerminal => self.firsts.add(TerminalRef::NullTerminal),
            _ => false
        }
    }
}

impl PartialEq for RuleChoice {
    fn eq(&self, other: &Self) -> bool {
        self.parts.len() == other.parts.len()
            && self
                .parts
                .iter()
                .zip(other.parts.iter())
                .all(|(e1, e2)| e1 == e2)
    }
}

impl Eq for RuleChoice {}

/// Represents the body of a grammar rule
#[derive(Debug, Clone, Default)]
pub struct RuleBody {
    /// The elements in this body
    pub parts: Vec<RuleBodyElement>,
    /// The FIRSTS set of terminals
    pub firsts: TerminalSet,
    /// The choices in this body
    pub choices: Vec<RuleChoice>
}

impl RuleBody {
    /// Initializes this rule body
    pub fn new(symbol: SymbolRef) -> RuleBody {
        RuleBody {
            parts: vec![RuleBodyElement::new(symbol, TREE_ACTION_NONE)],
            firsts: TerminalSet::default(),
            choices: Vec::new()
        }
    }

    /// Initializes this rule body from parts
    pub fn from_parts(parts: &[RuleBodyElement]) -> RuleBody {
        RuleBody {
            parts: parts.to_vec(),
            firsts: TerminalSet::default(),
            choices: Vec::new()
        }
    }

    /// Produces the concatenation of the left and right bodies
    pub fn concatenate(left: &RuleBody, right: &RuleBody) -> RuleBody {
        let mut parts = Vec::new();
        for element in left.parts.iter() {
            parts.push(*element);
        }
        for element in right.parts.iter() {
            parts.push(*element);
        }
        RuleBody {
            parts,
            firsts: TerminalSet::default(),
            choices: Vec::new()
        }
    }

    /// Gets the length of the rule choice
    pub fn len(&self) -> usize {
        self.parts.len()
    }

    /// Gets wether the rule choice is empty
    pub fn is_empty(&self) -> bool {
        self.parts.is_empty()
    }

    /// Appends a single symbol to the choice
    pub fn append_symbol(&mut self, symbol: SymbolRef) {
        self.parts
            .push(RuleBodyElement::new(symbol, TREE_ACTION_NONE));
    }

    /// Appends the content of another choice to this one
    pub fn append_choice(&mut self, other: &RuleChoice) {
        for element in other.parts.iter() {
            self.parts.push(*element);
        }
    }

    /// Applies the given action to all elements in this body
    pub fn apply_action(&mut self, action: TreeAction) {
        for element in self.parts.iter_mut() {
            element.action = action;
        }
    }

    /// Computes the FIRSTS set for this rule
    pub fn compute_firsts(&mut self, firsts_for_var: &mut HashMap<usize, TerminalSet>) -> bool {
        let mut modified = false;
        for i in (0..self.choices.len()).rev() {
            if i == self.choices.len() - 1 {
                modified |= self.choices[i].compute_firsts(&TerminalSet::default(), firsts_for_var);
            } else {
                let next_firsts = self.choices[i + 1].firsts.clone();
                modified |= self.choices[i].compute_firsts(&next_firsts, firsts_for_var);
            }
        }
        modified |= self.firsts.add_others(&self.choices[0].firsts);
        modified
    }

    /// Computes the choices for this rule body
    fn compute_choices(&mut self) {
        if self.choices.is_empty() {
            // For each part of the definition which is not a virtual symbol nor an action symbol
            for element in self.parts.iter() {
                match element.symbol {
                    SymbolRef::Virtual(_) => {}
                    SymbolRef::Action(_) => {}
                    _ => {
                        // Append the symbol to all the choices definition
                        for choice in self.choices.iter_mut() {
                            choice.append_symbol(element.symbol);
                        }
                        // Create a new choice with only the symbol
                        self.choices.push(RuleChoice::new(element.symbol));
                    }
                }
            }
            // Create a new empty choice
            self.choices.push(RuleChoice::default());
            self.firsts.add_others(&self.choices[0].firsts);
        }
    }

    /// Computes the initial FOLLOWERS sets
    pub fn compute_initial_follower(&self, followers: &mut HashMap<usize, TerminalSet>) {
        // For all choices but the last (empty)
        for (i, choice) in self.choices.iter().enumerate().take(self.choices.len() - 1) {
            if let SymbolRef::Variable(id) = choice.parts[0].symbol {
                // add the FIRSTS set of the next choice to the variable followers except ε
                for first in self.choices[i + 1].firsts.content.iter() {
                    if *first != TerminalRef::Epsilon {
                        followers.entry(id).or_default().add(*first);
                    }
                }
            }
        }
    }

    /// Propagate the followers
    pub fn propagate_followers(
        &self,
        head: usize,
        followers: &mut HashMap<usize, TerminalSet>
    ) -> bool {
        let mut modified = false;
        // For all choices but the last (empty)
        for (i, choice) in self.choices.iter().enumerate().take(self.choices.len() - 1) {
            if let SymbolRef::Variable(id) = choice.parts[0].symbol {
                // if the next choice FIRSTS set contains ε
                // add the FOLLOWERS of the head variable to the FOLLOWERS of the found variable
                if self.choices[i + 1]
                    .firsts
                    .content
                    .contains(&TerminalRef::Epsilon)
                {
                    let head_followers = followers.get(&head).cloned().unwrap_or_default();
                    modified |= followers.entry(id).or_default().add_others(&head_followers);
                }
            }
        }
        modified
    }
}

impl PartialEq for RuleBody {
    fn eq(&self, other: &Self) -> bool {
        self.parts.len() == other.parts.len()
            && self
                .parts
                .iter()
                .zip(other.parts.iter())
                .all(|(e1, e2)| e1 == e2)
    }
}

impl Eq for RuleBody {}

/// Represents a grammar rule
#[derive(Debug, Clone)]
pub struct Rule {
    /// The rule's head variable
    pub head: usize,
    /// The action on the rule's head
    pub head_action: TreeAction,
    /// The rule's body
    pub body: RuleBody,
    /// The lexical context pushed by this rule
    pub context: usize
}

impl Rule {
    /// Initializes this rule
    pub fn new(head: usize, head_action: TreeAction, body: RuleBody, context: usize) -> Rule {
        Rule {
            head,
            head_action,
            body,
            context
        }
    }
}

impl PartialEq for Rule {
    fn eq(&self, other: &Self) -> bool {
        self.head == other.head && self.body == other.body
    }
}

impl Eq for Rule {}

/// The prefix for the generated terminal names
pub const PREFIX_GENERATED_TERMINAL: &str = "__T";
/// The prefix for the generated variable names
pub const PREFIX_GENERATED_VARIABLE: &str = "__V";
/// The name of the generated axiom variable
pub const GENERATED_AXIOM: &str = "__VAxiom";
/// Name of the grammar option specifying the grammar's axiom variable
pub const OPTION_AXIOM: &str = "Axiom";
/// Name of the grammar option specifying the grammar's separator terminal
pub const OPTION_SEPARATOR: &str = "Separator";
/// The output path for compilation artifacts
pub const OPTION_OUTPUT_PATH: &str = "OutputPath";
/// The compilation mode to use, defaults to Source
pub const OPTION_COMPILATION_MODE: &str = "CompilationMode";
/// The parser type to generate, defaults to LALR1
pub const OPTION_PARSER_TYPE: &str = "ParserType";
/// The runtime to target, defaults to Net
pub const OPTION_RUNTIME: &str = "Runtime";
/// The namespace to use for the generated code
pub const OPTION_NAMESPACE: &str = "Namespace";
/// The access mode for the generated code, defaults to Internal
pub const OPTION_ACCESS_MODIFIER: &str = "AccessModifier";
/// The name of the default lexical context
pub const DEFAULT_CONTEXT_NAME: &str = "__default";

/// The counter for the generation of unique names across multiple grammars
static NEXT_UNIQUE_SID: AtomicUsize = AtomicUsize::new(0);

/// Generates a unique identifier
fn generate_unique_id() -> String {
    let value = NEXT_UNIQUE_SID.fetch_add(1, AtomicOrdering::SeqCst);
    format!("{:0X}", value)
}

/// Represents a grammar
#[derive(Debug, Clone)]
pub struct Grammar {
    /// The grammar's name
    pub name: String,
    /// The next unique symbol identifier for this grammar
    pub next_sid: usize,
    /// The grammar's options
    pub options: HashMap<String, String>,
    /// The lexical contexts defined in this grammar
    pub contexts: Vec<String>,
    /// The grammar's terminals
    pub terminals: Vec<Terminal>,
    /// The grammar's variables
    pub variables: Vec<Variable>,
    /// The grammar's virtual symbols
    pub virtuals: Vec<Virtual>,
    /// The grammar's action symbols
    pub actions: Vec<Action>
}

impl Grammar {
    /// Initializes this grammar
    pub fn new(name: &str) -> Grammar {
        Grammar {
            name: name.to_string(),
            next_sid: 3,
            options: HashMap::new(),
            contexts: vec![DEFAULT_CONTEXT_NAME.to_string()],
            terminals: Vec::new(),
            variables: Vec::new(),
            virtuals: Vec::new(),
            actions: Vec::new()
        }
    }
}
