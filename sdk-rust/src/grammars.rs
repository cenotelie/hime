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

use std::cmp::Ordering;
use std::collections::HashMap;
use std::sync::atomic::{AtomicUsize, Ordering as AtomicOrdering};

use hime_redist::parsers::{TreeAction, TREE_ACTION_DROP, TREE_ACTION_NONE, TREE_ACTION_PROMOTE};

use crate::errors::{Error, UnmatchableTokenError};
use crate::finite::{FinalItem, DFA, EPSILON, NFA};
use crate::lr::Graph;
use crate::sdk::InMemoryParser;
use crate::{InputReference, ParsingMethod};

/// Represents a symbol in a grammar
pub trait Symbol {
    /// Gets the unique indentifier (within a grammar) of this symbol
    fn get_id(&self) -> usize;

    /// Gets the name of this symbol
    fn get_name(&self) -> &str;

    /// Gets a description for this symbol
    fn get_description(&self) -> String;
}

/// A reference to a terminal in a terminal rule
#[derive(Debug, Clone)]
pub struct TerminalReference {
    /// The identifier of the referring terminal
    pub referring_id: usize,
    /// The input reference
    pub input_ref: InputReference
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
    /// The input reference for the definition
    pub input_ref: InputReference,
    /// The NFA that is used to match this terminal
    pub nfa: NFA,
    /// The context of this terminal
    pub context: usize,
    /// Whether the terminal is anonymous
    pub is_anonymous: bool,
    /// Whether the terminal is a fragment
    pub is_fragment: bool,
    /// The references to this terminal by others
    pub terminal_references: Vec<TerminalReference>
}

impl Terminal {
    /// Gets the priority of this terminal
    #[must_use]
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

    /// Gets a description for this symbol
    fn get_description(&self) -> String {
        if self.is_anonymous {
            format!("Inline terminal `{}`", &self.value)
        } else {
            format!(
                "Terminal {}`{}`",
                if self.is_fragment { "(fragment) " } else { "" },
                self.name
            )
        }
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
#[derive(Debug, Copy, Clone, Eq, PartialEq, Hash)]
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
    /// Gets the symbol id for the terminal
    #[must_use]
    pub fn sid(self) -> usize {
        match self {
            TerminalRef::Dummy | TerminalRef::NullTerminal => 0,
            TerminalRef::Epsilon => 1,
            TerminalRef::Dollar => 2,
            TerminalRef::Terminal(id) => id
        }
    }

    /// Gets the terminal priority
    #[must_use]
    pub fn priority(self) -> usize {
        self.sid()
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
#[derive(Debug, Clone, Default, Eq)]
pub struct TerminalSet {
    /// The backing content
    pub content: Vec<TerminalRef>
}

impl PartialEq for TerminalSet {
    fn eq(&self, other: &TerminalSet) -> bool {
        self.content.len() == other.content.len()
            && self
                .content
                .iter()
                .all(|t_ref| other.content.contains(t_ref))
    }
}

impl TerminalSet {
    /// Creates a set with a single element
    #[must_use]
    pub fn single(terminal: TerminalRef) -> TerminalSet {
        TerminalSet {
            content: vec![terminal]
        }
    }

    /// Gets the number of states in this automaton
    #[must_use]
    pub fn len(&self) -> usize {
        self.content.len()
    }

    /// Gets whether the NFA has no state
    #[must_use]
    pub fn is_empty(&self) -> bool {
        self.content.is_empty()
    }

    /// Adds a new terminal
    fn do_add(&mut self, item: TerminalRef) -> bool {
        if self.content.contains(&item) {
            false
        } else {
            self.content.push(item);
            true
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
        for item in &others.content {
            modified |= self.do_add(*item);
        }
        self.content.sort();
        modified
    }

    /// Removes all items from this collection
    pub fn clear(&mut self) {
        self.content.clear();
    }

    /// Sorts this set by priority
    pub fn sort(&mut self) {
        self.content.sort();
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
    #[must_use]
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

    /// Gets a description for this symbol
    fn get_description(&self) -> String {
        format!("Virtual symbol `{}`", &self.name)
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
    #[must_use]
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

    /// Gets a description for this symbol
    fn get_description(&self) -> String {
        format!("Action `{}`", &self.name)
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
    /// If the variable was generated, the identifier of the variable in the context of which it was generated
    pub generated_for: Option<usize>,
    /// The rules for this variable
    pub rules: Vec<Rule>,
    /// The FIRSTS set for this variable
    pub firsts: TerminalSet,
    /// The FOLLOWERS set for this variable
    pub followers: TerminalSet
}

impl Variable {
    /// Creates a new variable
    #[must_use]
    pub fn new(id: usize, name: String, generated_for: Option<usize>) -> Variable {
        Variable {
            id,
            name,
            generated_for,
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
        for rule in &mut self.rules {
            rule.body.compute_choices();
        }
    }

    /// Computes the FIRSTS set for this variable
    pub fn compute_firsts(&mut self, firsts_for_var: &mut HashMap<usize, TerminalSet>) -> bool {
        let mut modified = false;
        for rule in &mut self.rules {
            modified |= self.firsts.add_others(&rule.body.firsts);
            modified |= firsts_for_var
                .entry(self.id)
                .or_insert_with(TerminalSet::default)
                .add_others(&rule.body.firsts);
            modified |= rule.body.compute_firsts(firsts_for_var);
        }
        modified
    }

    /// Computes the initial FOLLOWERS sets
    pub fn compute_initial_follower(&self, followers: &mut HashMap<usize, TerminalSet>) {
        for rule in &self.rules {
            rule.body.compute_initial_follower(followers);
        }
    }

    /// Propagate the followers
    pub fn propagate_followers(&self, followers: &mut HashMap<usize, TerminalSet>) -> bool {
        let mut modified = false;
        for rule in &self.rules {
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

    /// Gets a description for this symbol
    fn get_description(&self) -> String {
        format!("Variable `{}`", &self.name)
    }
}

impl PartialEq for Variable {
    fn eq(&self, other: &Self) -> bool {
        self.id == other.id
    }
}

impl Eq for Variable {}

/// Represents a reference to a grammar symbol
#[derive(Debug, Copy, Clone, Eq, PartialEq, Hash)]
pub enum SymbolRef {
    /// Represents a fake terminal, used as a marker by LR-related algorithms
    Dummy,
    /// Represents the epsilon symbol in a grammar, i.e. a terminal with an empty value
    Epsilon,
    /// Represents the dollar symbol in a grammar, i.e. the marker of end of input
    Dollar,
    /// Represents the absence of terminal, used as a marker by LR-related algorithms
    NullTerminal,
    /// A terminal in a grammar
    Terminal(usize),
    /// A variable in a grammar
    Variable(usize),
    /// A virtual symbol in a grammar
    Virtual(usize),
    /// An action symbol in a grammar
    Action(usize)
}

impl SymbolRef {
    /// Gets the terminal priority
    #[must_use]
    pub fn priority(self) -> usize {
        match self {
            SymbolRef::Dummy | SymbolRef::NullTerminal => 0,
            SymbolRef::Epsilon => 1,
            SymbolRef::Dollar => 2,
            SymbolRef::Terminal(id)
            | SymbolRef::Variable(id)
            | SymbolRef::Virtual(id)
            | SymbolRef::Action(id) => id
        }
    }
}

impl Ord for SymbolRef {
    fn cmp(&self, other: &SymbolRef) -> Ordering {
        self.priority().cmp(&other.priority())
    }
}

impl PartialOrd for SymbolRef {
    fn partial_cmp(&self, other: &SymbolRef) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

impl From<TerminalRef> for SymbolRef {
    fn from(terminal: TerminalRef) -> Self {
        match terminal {
            TerminalRef::Dummy => SymbolRef::Dummy,
            TerminalRef::Epsilon => SymbolRef::Epsilon,
            TerminalRef::Dollar => SymbolRef::Dollar,
            TerminalRef::NullTerminal => SymbolRef::NullTerminal,
            TerminalRef::Terminal(id) => SymbolRef::Terminal(id)
        }
    }
}

/// Represents an element in the body of a grammar rule
#[derive(Debug, Copy, Clone)]
pub struct RuleBodyElement {
    /// The symbol of this element
    pub symbol: SymbolRef,
    /// The action applied on this element
    pub action: TreeAction,
    /// The reference to this body element in the input
    pub input_ref: Option<InputReference>
}

impl PartialEq for RuleBodyElement {
    fn eq(&self, other: &Self) -> bool {
        self.symbol == other.symbol && self.action == other.action
    }
}

impl RuleBodyElement {
    /// Creates a new body element
    #[must_use]
    pub fn new(
        symbol: SymbolRef,
        action: TreeAction,
        input_ref: Option<InputReference>
    ) -> RuleBodyElement {
        RuleBodyElement {
            symbol,
            action,
            input_ref
        }
    }

    /// Gets a version of this element without the action
    #[must_use]
    pub fn no_action(&self) -> RuleBodyElement {
        RuleBodyElement {
            symbol: self.symbol,
            action: TREE_ACTION_NONE,
            input_ref: self.input_ref
        }
    }
}

/// Represents a choice in a rule, i.e. the remainder of a rule's body
#[derive(Debug, Clone, Default)]
pub struct RuleChoice {
    /// The elements in this body
    pub elements: Vec<RuleBodyElement>,
    /// The FIRSTS set of terminals
    pub firsts: TerminalSet
}

impl RuleChoice {
    /// Creates a new choice from a single symbol
    #[must_use]
    pub fn from_single_part(element: &RuleBodyElement) -> RuleChoice {
        RuleChoice {
            elements: vec![element.no_action()],
            firsts: TerminalSet::default()
        }
    }

    /// Initializes this rule body from elements
    #[must_use]
    pub fn from_parts(elements: Vec<RuleBodyElement>) -> RuleBody {
        RuleBody {
            elements,
            firsts: TerminalSet::default(),
            choices: Vec::new()
        }
    }

    /// Gets the length of the rule choice
    #[must_use]
    pub fn len(&self) -> usize {
        self.elements.len()
    }

    /// Gets wether the rule choice is empty
    #[must_use]
    pub fn is_empty(&self) -> bool {
        self.elements.is_empty()
    }

    /// Appends a single symbol to the choice
    pub fn append_part(&mut self, element: &RuleBodyElement) {
        self.elements.push(element.no_action());
    }

    /// Appends the content of another choice to this one
    pub fn append_choice(&mut self, other: &RuleChoice) {
        for element in &other.elements {
            self.elements.push(*element);
        }
    }

    /// Computes the FIRSTS set for this rule choice
    pub fn compute_firsts(
        &mut self,
        next: &TerminalSet,
        firsts_for_var: &mut HashMap<usize, TerminalSet>
    ) -> bool {
        // If the choice is empty : Add the ε to the Firsts and return
        if self.elements.is_empty() {
            return self.firsts.add(TerminalRef::Epsilon);
        }
        match self.elements[0].symbol {
            SymbolRef::Variable(id) => {
                let mut modified = false;
                if let Some(var_firsts) = firsts_for_var.get(&id) {
                    for first in &var_firsts.content {
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
        self.elements.len() == other.elements.len()
            && self
                .elements
                .iter()
                .zip(other.elements.iter())
                .all(|(e1, e2)| e1 == e2)
    }
}

impl Eq for RuleChoice {}

/// Common trait for different kind of rule body
pub trait RuleBodyTrait {
    /// Produces the concatenation of two elements
    fn concatenate(left: &Self, right: &Self) -> Self;

    /// Apply a tree action to all elements in the body
    fn apply_action(&mut self, action: TreeAction);
}

/// A set of rule bodies
pub struct BodySet<T: RuleBodyTrait> {
    /// The bodies in the set
    pub bodies: Vec<T>
}

impl<T: RuleBodyTrait> BodySet<T> {
    /// Builds the union of the left and right set
    #[must_use]
    pub fn union(mut left: BodySet<T>, mut right: BodySet<T>) -> BodySet<T> {
        let mut bodies = Vec::with_capacity(left.bodies.len() + right.bodies.len());
        bodies.append(&mut left.bodies);
        bodies.append(&mut right.bodies);
        BodySet { bodies }
    }

    /// Builds the product of the left and right set
    #[must_use]
    pub fn product(left: BodySet<T>, right: &BodySet<T>) -> BodySet<T> {
        let mut bodies = Vec::with_capacity(left.bodies.len() * right.bodies.len());
        for body_left in left.bodies {
            for body_right in &right.bodies {
                bodies.push(T::concatenate(&body_left, body_right));
            }
        }
        BodySet { bodies }
    }

    /// Apply a tree action to all elements in the bodies
    pub fn apply_action(&mut self, action: TreeAction) {
        for body in &mut self.bodies {
            body.apply_action(action);
        }
    }
}

/// Represents the body of a grammar rule
#[derive(Debug, Clone, Default)]
pub struct RuleBody {
    /// The elements in this body
    pub elements: Vec<RuleBodyElement>,
    /// The FIRSTS set of terminals
    pub firsts: TerminalSet,
    /// The choices in this body
    pub choices: Vec<RuleChoice>
}

impl RuleBodyTrait for RuleBody {
    fn concatenate(left: &RuleBody, right: &RuleBody) -> RuleBody {
        let mut elements = Vec::with_capacity(left.elements.len() + right.elements.len());
        for element in &left.elements {
            elements.push(*element);
        }
        for element in &right.elements {
            elements.push(*element);
        }
        RuleBody {
            elements,
            firsts: TerminalSet::default(),
            choices: Vec::new()
        }
    }

    fn apply_action(&mut self, action: TreeAction) {
        for element in &mut self.elements {
            element.action = action;
        }
    }
}

impl RuleBody {
    /// Initializes this rule body
    #[must_use]
    pub fn empty() -> RuleBody {
        RuleBody {
            elements: Vec::new(),
            firsts: TerminalSet::default(),
            choices: Vec::new()
        }
    }

    /// Initializes this rule body
    #[must_use]
    pub fn single(symbol: SymbolRef, input_ref: InputReference) -> RuleBody {
        RuleBody {
            elements: vec![RuleBodyElement::new(
                symbol,
                TREE_ACTION_NONE,
                Some(input_ref)
            )],
            firsts: TerminalSet::default(),
            choices: Vec::new()
        }
    }

    /// Initializes this rule body from elements
    #[must_use]
    pub fn from_parts(elements: Vec<RuleBodyElement>) -> RuleBody {
        RuleBody {
            elements,
            firsts: TerminalSet::default(),
            choices: Vec::new()
        }
    }

    /// Gets the length of the rule choice
    #[must_use]
    pub fn len(&self) -> usize {
        self.elements.len()
    }

    /// Gets wether the rule choice is empty
    #[must_use]
    pub fn is_empty(&self) -> bool {
        self.elements.is_empty()
    }

    /// Appends the content of another choice to this one
    pub fn append_choice(&mut self, other: &RuleChoice) {
        for element in &other.elements {
            self.elements.push(*element);
        }
    }

    /// Applies the given action to all elements in this body
    pub fn apply_action(&mut self, action: TreeAction) {
        for element in &mut self.elements {
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
            // For each element of the definition which is not a virtual symbol nor an action symbol
            for element in &self.elements {
                match element.symbol {
                    SymbolRef::Virtual(_) | SymbolRef::Action(_) => {}
                    _ => {
                        // Append the symbol to all the choices definition
                        for choice in &mut self.choices {
                            choice.append_part(element);
                        }
                        // Create a new choice with only the symbol
                        self.choices.push(RuleChoice::from_single_part(element));
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
            if let SymbolRef::Variable(id) = choice.elements[0].symbol {
                // add the FIRSTS set of the next choice to the variable followers except ε
                for first in &self.choices[i + 1].firsts.content {
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
            if let SymbolRef::Variable(id) = choice.elements[0].symbol {
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
        self.elements.len() == other.elements.len()
            && self
                .elements
                .iter()
                .zip(other.elements.iter())
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
    /// The input reference for the rule's head
    pub head_input_ref: InputReference,
    /// The rule's body
    pub body: RuleBody,
    /// The lexical context pushed by this rule
    pub context: usize
}

impl Rule {
    /// Initializes this rule
    #[must_use]
    pub fn new(
        head: usize,
        head_action: TreeAction,
        input_ref: InputReference,
        body: RuleBody,
        context: usize
    ) -> Rule {
        Rule {
            head,
            head_action,
            head_input_ref: input_ref,
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

/// A reference to a grammar rule
#[derive(Debug, Copy, Clone, PartialEq, Eq)]
pub struct RuleRef {
    /// The identifier of the variable
    pub variable: usize,
    /// The index of the rule for the variable
    pub index: usize
}

impl RuleRef {
    /// Creates a new rule reference
    #[must_use]
    pub fn new(variable: usize, index: usize) -> RuleRef {
        RuleRef { variable, index }
    }

    /// Gets the referenced rule in the grammar
    ///
    /// # Panics
    ///
    /// Panic when the rule's head cannot be found in the grammar
    #[must_use]
    pub fn get_rule_in<'g>(&self, grammar: &'g Grammar) -> &'g Rule {
        &grammar
            .variables
            .iter()
            .find(|v| v.id == self.variable)
            .unwrap()
            .rules[self.index]
    }
}

/// A reference to a choice in a grammar rule
#[derive(Debug, Copy, Clone, PartialEq, Eq)]
pub struct RuleChoiceRef {
    /// The associated rule
    pub rule: RuleRef,
    /// The position within the rule
    pub position: usize
}

impl RuleChoiceRef {
    /// Creates a new choice reference
    #[must_use]
    pub fn new(variable: usize, index: usize, position: usize) -> RuleChoiceRef {
        RuleChoiceRef {
            rule: RuleRef::new(variable, index),
            position
        }
    }
}

/// Reference to a template rule
#[derive(Debug, Clone, Eq, PartialEq)]
pub struct TemplateRuleRef {
    /// Index of the template rule
    pub template: usize,
    /// The input reference for this call
    pub input_ref: InputReference,
    /// The arguments to the template
    pub arguments: Vec<TemplateRuleSymbol>
}

/// A symbol in a template rule
#[derive(Debug, Clone, Eq, PartialEq)]
pub enum TemplateRuleSymbol {
    /// A reference to the n-th parameter of the template
    Parameter(usize),
    /// A usual reference to a grammar symbol
    Symbol(SymbolRef),
    /// A call to template rule
    Template(TemplateRuleRef)
}

/// An element in a template rule
#[derive(Debug, Clone)]
pub struct TemplateRuleElement {
    /// The symbol of this element
    pub symbol: TemplateRuleSymbol,
    /// The action applied on this element
    pub action: TreeAction,
    /// The reference to this body element in the input
    pub input_ref: InputReference
}

impl PartialEq for TemplateRuleElement {
    fn eq(&self, other: &Self) -> bool {
        self.symbol == other.symbol && self.action == other.action
    }
}

impl TemplateRuleElement {
    /// Creates a new body element
    #[must_use]
    pub fn new(
        symbol: TemplateRuleSymbol,
        action: TreeAction,
        input_ref: InputReference
    ) -> TemplateRuleElement {
        TemplateRuleElement {
            symbol,
            action,
            input_ref
        }
    }
}

/// An instance of a template rule
#[derive(Debug, Clone)]
pub struct TemplateRuleInstance {
    /// The arguments used for this instance
    pub arguments: Vec<SymbolRef>,
    /// The identifier of the produced head variable
    pub head: usize
}

impl PartialEq for TemplateRuleInstance {
    fn eq(&self, other: &Self) -> bool {
        self.arguments.len() == other.arguments.len()
            && self
                .arguments
                .iter()
                .zip(other.arguments.iter())
                .all(|(arg1, arg2)| arg1 == arg2)
    }
}

impl Eq for TemplateRuleInstance {}

/// A body for a template rule
#[derive(Debug, Clone)]
pub struct TemplateRuleBody {
    /// The elements in the rule's body
    pub elements: Vec<TemplateRuleElement>
}

impl RuleBodyTrait for TemplateRuleBody {
    fn concatenate(left: &TemplateRuleBody, right: &TemplateRuleBody) -> TemplateRuleBody {
        let mut elements = Vec::with_capacity(left.elements.len() + right.elements.len());
        for element in &left.elements {
            elements.push(element.clone());
        }
        for element in &right.elements {
            elements.push(element.clone());
        }
        TemplateRuleBody { elements }
    }

    fn apply_action(&mut self, action: TreeAction) {
        for element in &mut self.elements {
            element.action = action;
        }
    }
}

impl TemplateRuleBody {
    /// Initializes this rule body
    #[must_use]
    pub fn empty() -> TemplateRuleBody {
        TemplateRuleBody {
            elements: Vec::new()
        }
    }

    /// Initializes this rule body
    #[must_use]
    pub fn single(symbol: TemplateRuleSymbol, input_ref: InputReference) -> TemplateRuleBody {
        TemplateRuleBody {
            elements: vec![TemplateRuleElement::new(
                symbol,
                TREE_ACTION_NONE,
                input_ref
            )]
        }
    }
}

/// A parameter for the template rule
#[derive(Debug, Clone)]
pub struct TemplateRuleParam {
    /// The name of the parameter
    pub name: String,
    /// The input reference
    pub input_ref: InputReference
}

/// A template rule in a grammar
#[derive(Debug, Clone)]
pub struct TemplateRule {
    /// The base head name for the rule
    pub name: String,
    /// The input reference for the definition
    pub input_ref: InputReference,
    /// The name of the parameters for the rule
    pub parameters: Vec<TemplateRuleParam>,
    /// The action on the rule's head
    pub head_action: TreeAction,
    /// The lexical context pushed by this rule
    pub context: usize,
    /// The possible bodies for the template rule
    pub bodies: Vec<TemplateRuleBody>,
    /// The known instanes of this rule
    pub instances: Vec<TemplateRuleInstance>
}

impl TemplateRule {
    /// Initializes a new template rule
    #[must_use]
    pub fn new(
        name: String,
        input_ref: InputReference,
        parameters: Vec<TemplateRuleParam>
    ) -> TemplateRule {
        TemplateRule {
            name,
            input_ref,
            parameters,
            head_action: TREE_ACTION_NONE,
            context: 0,
            bodies: Vec::new(),
            instances: Vec::new()
        }
    }

    /// Determines whether an instance for the specified arguments already exist
    #[must_use]
    pub fn has_instance(&self, arguments: &[SymbolRef]) -> bool {
        self.instances.iter().any(|instance| {
            instance.arguments.len() == arguments.len()
                && instance
                    .arguments
                    .iter()
                    .zip(arguments.iter())
                    .all(|(a1, a2)| a1 == a2)
        })
    }
}

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
/// The parser type to generate, defaults to LALR1
pub const OPTION_METHOD: &str = "Method";
/// The runtime to target, defaults to Net
pub const OPTION_RUNTIME: &str = "Runtime";
/// The compilation mode, defaults to Sources
pub const OPTION_MODE: &str = "Mode";
/// The namespace to use for the generated code
pub const OPTION_NAMESPACE: &str = "Namespace";
/// The access mode for the generated code, defaults to Internal
pub const OPTION_ACCESS_MODIFIER: &str = "Modifier";
/// The name of the default lexical context
pub const DEFAULT_CONTEXT_NAME: &str = "__default";

/// The counter for the generation of unique names across multiple grammars
static NEXT_UNIQUE_SID: AtomicUsize = AtomicUsize::new(0);

/// Generates a unique identifier
fn generate_unique_id() -> String {
    let value = NEXT_UNIQUE_SID.fetch_add(1, AtomicOrdering::SeqCst);
    format!("{value:0X}")
}

/// An option for the grammar
#[derive(Debug, Clone)]
pub struct GrammarOption {
    /// The reference in the input for this option's name
    pub name_input_ref: InputReference,
    /// The reference in the input for this option's value
    pub value_input_ref: InputReference,
    /// The option's value
    pub value: String
}

/// Represents a grammar
#[derive(Debug, Clone)]
pub struct Grammar {
    /// The reference in the input for this grammar
    pub input_ref: InputReference,
    /// The grammar's name
    pub name: String,
    /// The next unique symbol identifier for this grammar
    pub next_sid: usize,
    /// The grammar's options
    pub options: HashMap<String, GrammarOption>,
    /// The lexical contexts defined in this grammar
    pub contexts: Vec<String>,
    /// The grammar's terminals
    pub terminals: Vec<Terminal>,
    /// The grammar's variables
    pub variables: Vec<Variable>,
    /// The grammar's virtual symbols
    pub virtuals: Vec<Virtual>,
    /// The grammar's action symbols
    pub actions: Vec<Action>,
    /// The template rules
    pub template_rules: Vec<TemplateRule>
}

/// Represents the build data for a grammar
#[derive(Debug, Clone)]
pub struct BuildData {
    /// The DFA
    pub dfa: DFA,
    /// The expected terminals
    pub expected: TerminalSet,
    /// The separator terminal
    pub separator: Option<TerminalRef>,
    /// The parsing method
    pub method: ParsingMethod,
    /// The LR graph
    pub graph: Graph
}

impl Grammar {
    /// Initializes this grammar
    #[must_use]
    pub fn new(input_ref: InputReference, name: String) -> Grammar {
        Grammar {
            input_ref,
            name,
            next_sid: 3,
            options: HashMap::new(),
            contexts: vec![DEFAULT_CONTEXT_NAME.to_string()],
            terminals: Vec::new(),
            variables: Vec::new(),
            virtuals: Vec::new(),
            actions: Vec::new(),
            template_rules: Vec::new()
        }
    }

    /// Gets the next available symbol id
    fn get_next_sid(&mut self) -> usize {
        let result = self.next_sid;
        self.next_sid += 1;
        result
    }

    /// Adds an option to this grammar
    pub fn add_option(
        &mut self,
        name_input_ref: InputReference,
        value_input_ref: InputReference,
        name: String,
        value: String
    ) {
        self.options.insert(
            name,
            GrammarOption {
                name_input_ref,
                value_input_ref,
                value
            }
        );
    }

    /// Gets an option
    #[must_use]
    pub fn get_option(&self, name: &str) -> Option<&GrammarOption> {
        self.options.get(name)
    }

    /// Gets the symbol with the given name in this grammar
    #[must_use]
    pub fn get_symbol(&self, name: &str) -> Option<SymbolRef> {
        if let Some(symbol) = self.terminals.iter().find(|t| t.name == name) {
            return Some(SymbolRef::Terminal(symbol.id));
        }
        if let Some(symbol) = self.variables.iter().find(|t| t.name == name) {
            return Some(SymbolRef::Variable(symbol.id));
        }
        if let Some(symbol) = self.virtuals.iter().find(|t| t.name == name) {
            return Some(SymbolRef::Virtual(symbol.id));
        }
        if let Some(symbol) = self.actions.iter().find(|t| t.name == name) {
            return Some(SymbolRef::Action(symbol.id));
        }
        None
    }

    /// Gets the name of a symbol
    ///
    /// # Panics
    ///
    /// Panic when the symbol could not be found in this grammar
    #[must_use]
    pub fn get_symbol_name(&self, symbol: SymbolRef) -> &str {
        match symbol {
            SymbolRef::Dummy | SymbolRef::NullTerminal => "",
            SymbolRef::Epsilon => "ε",
            SymbolRef::Dollar => "$",
            SymbolRef::Terminal(id) => self
                .terminals
                .iter()
                .find(|s| s.id == id)
                .map(|s| &s.name)
                .unwrap(),
            SymbolRef::Variable(id) => self
                .variables
                .iter()
                .find(|s| s.id == id)
                .map(|s| &s.name)
                .unwrap(),
            SymbolRef::Virtual(id) => self
                .virtuals
                .iter()
                .find(|s| s.id == id)
                .map(|s| &s.name)
                .unwrap(),
            SymbolRef::Action(id) => self
                .actions
                .iter()
                .find(|s| s.id == id)
                .map(|s| &s.name)
                .unwrap()
        }
    }

    /// Gets the value of a symbol
    ///
    /// # Panics
    ///
    /// Panic when the symbol could not be found in this grammar
    #[must_use]
    pub fn get_symbol_value(&self, symbol: SymbolRef) -> &str {
        match symbol {
            SymbolRef::Terminal(id) => self
                .terminals
                .iter()
                .find(|s| s.id == id)
                .map(|s| &s.value)
                .unwrap(),
            _ => self.get_symbol_name(symbol)
        }
    }

    /// Resolves the specified lexical context name for this grammar
    pub fn resolve_context(&mut self, name: &str) -> usize {
        if let Some(index) = self.contexts.iter().position(|c| name == c) {
            index
        } else {
            let index = self.contexts.len();
            self.contexts.push(name.to_string());
            index
        }
    }

    /// Adds the given anonymous terminal to this grammar
    pub fn add_terminal_anonymous(
        &mut self,
        value: String,
        input_ref: InputReference,
        nfa: NFA
    ) -> &mut Terminal {
        let name = format!("{}{}", PREFIX_GENERATED_TERMINAL, generate_unique_id());
        self.add_terminal(name, value, input_ref, nfa, 0, true, false)
    }

    /// Adds the given named terminal to this grammar
    ///
    /// # Panics
    ///
    /// Panic when the specified context does not exist in the grammar
    pub fn add_terminal_named(
        &mut self,
        name: String,
        input_ref: InputReference,
        nfa: NFA,
        context: &str,
        is_fragment: bool
    ) -> &mut Terminal {
        let context = self.contexts.iter().position(|c| c == context).unwrap();
        let value = name.clone();
        self.add_terminal(name, value, input_ref, nfa, context, false, is_fragment)
    }

    /// Adds a terminal to the grammar
    #[allow(clippy::too_many_arguments)]
    fn add_terminal(
        &mut self,
        name: String,
        value: String,
        input_ref: InputReference,
        nfa: NFA,
        context: usize,
        is_anonymous: bool,
        is_fragment: bool
    ) -> &mut Terminal {
        let index = self.terminals.len();
        let terminal = Terminal {
            id: self.get_next_sid(),
            name,
            value,
            input_ref,
            nfa,
            context,
            is_anonymous,
            is_fragment,
            terminal_references: Vec::new()
        };
        self.terminals.push(terminal);
        &mut self.terminals[index]
    }

    /// Gets the terminal with the specified identifier
    #[must_use]
    pub fn get_terminal(&self, sid: usize) -> Option<&Terminal> {
        self.terminals.iter().find(|t| t.id == sid)
    }

    /// Gets the terminal with the specified identifier
    pub fn get_terminal_mut(&mut self, sid: usize) -> Option<&mut Terminal> {
        self.terminals.iter_mut().find(|t| t.id == sid)
    }

    /// Gets the terminal with the given name
    #[must_use]
    pub fn get_terminal_for_name(&self, name: &str) -> Option<&Terminal> {
        self.terminals.iter().find(|t| t.name == name)
    }

    /// Gets the terminal with the given name
    #[must_use]
    pub fn get_terminal_for_value(&self, value: &str) -> Option<&Terminal> {
        self.terminals.iter().find(|t| t.value == value)
    }

    /// Gets the context for a terminal
    ///
    /// # Panics
    ///
    /// Panic when the terminal cannot be found in the grammar
    #[must_use]
    pub fn get_terminal_context(&self, terminal: TerminalRef) -> usize {
        match terminal {
            TerminalRef::Dummy
            | TerminalRef::Epsilon
            | TerminalRef::Dollar
            | TerminalRef::NullTerminal => 0,
            TerminalRef::Terminal(id) => self.terminals.iter().find(|t| t.id == id).unwrap().context
        }
    }

    /// Generates a new variable
    pub fn generate_variable(&mut self, context_variable: usize) -> &mut Variable {
        let index = self.variables.len();
        let sid = self.get_next_sid();
        let name = format!("{PREFIX_GENERATED_VARIABLE}{sid}");
        self.variables
            .push(Variable::new(sid, name, Some(context_variable)));
        &mut self.variables[index]
    }

    /// Gets the variable with the specified identifier
    #[must_use]
    pub fn get_variable(&self, sid: usize) -> Option<&Variable> {
        self.variables.iter().find(|v| v.id == sid)
    }

    /// Gets the variable with the specified name
    #[must_use]
    pub fn get_variable_for_name(&self, name: &str) -> Option<&Variable> {
        self.variables.iter().find(|v| v.name == name)
    }

    /// Adds a variable with the given name to this grammar
    pub fn add_variable(&mut self, name: &str) -> &mut Variable {
        if let Some(index) = self.variables.iter().position(|v| v.name == name) {
            return &mut self.variables[index];
        }
        let index = self.variables.len();
        let sid = self.get_next_sid();
        self.variables
            .push(Variable::new(sid, name.to_string(), None));
        &mut self.variables[index]
    }

    /// Inherit the specified variable
    fn inherit_variable(&mut self, other: &Variable) {
        if self.variables.iter().all(|v| v.name != other.name) {
            // no variable with the same name
            let sid = self.next_sid + other.id - 3;
            self.variables
                .push(Variable::new(sid, other.name.clone(), None));
        }
    }

    /// Gets the virtual with the specified identifier
    #[must_use]
    pub fn get_virtual(&self, sid: usize) -> Option<&Virtual> {
        self.virtuals.iter().find(|v| v.id == sid)
    }

    /// Adds a virtual symbol with the given name to this grammar
    pub fn add_virtual(&mut self, name: &str) -> &mut Virtual {
        if let Some(index) = self.virtuals.iter().position(|v| v.name == name) {
            return &mut self.virtuals[index];
        }
        let index = self.virtuals.len();
        let sid = self.get_next_sid();
        self.virtuals.push(Virtual::new(sid, name.to_string()));
        &mut self.virtuals[index]
    }

    /// Inherit the specified virtual
    fn inherit_virtal(&mut self, other: &Virtual) {
        if self.virtuals.iter().all(|v| v.name != other.name) {
            // no variable with the same name
            let sid = self.next_sid + other.id - 3;
            self.virtuals.push(Virtual::new(sid, other.name.clone()));
        }
    }

    /// Gets the action with the specified identifier
    #[must_use]
    pub fn get_action(&self, sid: usize) -> Option<&Action> {
        self.actions.iter().find(|v| v.id == sid)
    }

    /// Adds an action symbol with the given name to this grammar
    pub fn add_action(&mut self, name: &str) -> &mut Action {
        if let Some(index) = self.actions.iter().position(|v| v.name == name) {
            return &mut self.actions[index];
        }
        let index = self.actions.len();
        let sid = self.get_next_sid();
        self.actions.push(Action::new(sid, name.to_string()));
        &mut self.actions[index]
    }

    /// Inherit the specified action
    fn inherit_action(&mut self, other: &Action) {
        if self.actions.iter().all(|v| v.name != other.name) {
            // no variable with the same name
            let sid = self.next_sid + other.id - 3;
            self.actions.push(Action::new(sid, other.name.clone()));
        }
    }

    /// Adds a template rule with the given name to this grammar
    pub fn add_template_rule(
        &mut self,
        name: &str,
        input_ref: InputReference,
        parameters: Vec<TemplateRuleParam>
    ) -> &mut TemplateRule {
        if let Some(index) = self.template_rules.iter().position(|v| v.name == name) {
            return &mut self.template_rules[index];
        }
        let index = self.template_rules.len();
        self.template_rules
            .push(TemplateRule::new(name.to_string(), input_ref, parameters));
        &mut self.template_rules[index]
    }

    /// Generates a new template rule
    pub fn generate_template_rule(
        &mut self,
        input_ref: InputReference,
        parameters: Vec<TemplateRuleParam>
    ) -> &mut TemplateRule {
        let sid = self.get_next_sid();
        let name = format!("{PREFIX_GENERATED_VARIABLE}{sid}");
        self.add_template_rule(&name, input_ref, parameters)
    }

    /// Instantiate a template rule
    ///
    /// # Errors
    ///
    /// Return an error when the wrong number of arguments are passed to the template rule
    pub fn instantiate_template_rule(
        &mut self,
        name: &str,
        call_ref: InputReference,
        arguments: Vec<SymbolRef>
    ) -> Result<SymbolRef, Error> {
        match self.template_rules.iter().position(|r| r.name == name) {
            None => Err(Error::TemplateRuleNotFound(call_ref, name.to_string())),
            Some(template_index) => {
                let rule = &self.template_rules[template_index];
                if rule.parameters.len() == arguments.len() {
                    Ok(self.instantiate_template_rule_at(template_index, call_ref, arguments))
                } else {
                    Err(Error::TemplateRuleWrongNumberOfArgs(
                        call_ref,
                        rule.parameters.len(),
                        arguments.len()
                    ))
                }
            }
        }
    }

    /// Instantiate a symbol in a template rule
    fn instantiate_template_symbol(
        &mut self,
        template_index: usize,
        instance_index: usize,
        symbol: &TemplateRuleSymbol
    ) -> SymbolRef {
        match symbol {
            TemplateRuleSymbol::Parameter(index) => {
                self.template_rules[template_index].instances[instance_index].arguments[*index]
            }
            TemplateRuleSymbol::Symbol(symbol) => *symbol,
            TemplateRuleSymbol::Template(template_ref) => {
                let mut new_arguments = Vec::new();
                for arg in &template_ref.arguments {
                    new_arguments.push(self.instantiate_template_symbol(
                        template_index,
                        instance_index,
                        arg
                    ));
                }
                self.instantiate_template_rule_at(
                    template_ref.template,
                    template_ref.input_ref,
                    new_arguments
                )
            }
        }
    }

    /// Instantiate a template rule
    fn instantiate_template_rule_at(
        &mut self,
        template_index: usize,
        call_ref: InputReference,
        arguments: Vec<SymbolRef>
    ) -> SymbolRef {
        let mut new_instance = TemplateRuleInstance { arguments, head: 0 };
        if let Some(instance) = self.template_rules[template_index]
            .instances
            .iter()
            .find(|instance| *instance == &new_instance)
        {
            SymbolRef::Variable(instance.head)
        } else {
            // push new instance
            let args_names: Vec<&str> = new_instance
                .arguments
                .iter()
                .map(|arg| self.get_symbol_name(*arg))
                .collect();
            let args_names = args_names.join(", ");
            let name = format!(
                "{}<{}>",
                &self.template_rules[template_index].name, args_names
            );
            new_instance.head = self.add_variable(&name).id;
            let instance_index = self.template_rules[template_index].instances.len();
            self.template_rules[template_index]
                .instances
                .push(new_instance);

            // fill-in the body
            let head_action = self.template_rules[template_index].head_action;
            let context = self.template_rules[template_index].context;
            let mut bodies = Vec::new();
            for body in self.template_rules[template_index].bodies.clone() {
                let mut elements = Vec::new();
                for element in body.elements {
                    elements.push(RuleBodyElement {
                        symbol: self.instantiate_template_symbol(
                            template_index,
                            instance_index,
                            &element.symbol
                        ),
                        action: element.action,
                        input_ref: Some(element.input_ref)
                    });
                }
                bodies.push(RuleBody::from_parts(elements));
            }
            let head = {
                let variable = self.add_variable(&name);
                for body in bodies {
                    variable.rules.push(Rule {
                        head: variable.id,
                        head_action,
                        head_input_ref: call_ref,
                        body,
                        context
                    });
                }
                variable.id
            };

            SymbolRef::Variable(head)
        }
    }

    /// Inherit from the given parent
    pub fn inherit(&mut self, other: &Grammar) {
        self.inherit_options(other);
        self.inherit_terminals(other);
        self.inherit_variables(other);
        self.inherit_virtuals(other);
        self.inherit_actions(other);
        self.inherit_rules(other);
        self.inherit_template_rules(other);
        self.next_sid += other.next_sid - 3;
    }

    /// Inherits the options from the parent grammar
    fn inherit_options(&mut self, other: &Grammar) {
        for (name, option) in &other.options {
            self.options.insert(name.clone(), option.clone());
        }
    }

    /// Inherits the terminals from the parent grammar
    fn inherit_terminals(&mut self, other: &Grammar) {
        for terminal in &other.terminals {
            if self
                .terminals
                .iter()
                .all(|t| t.name != terminal.name && t.value != terminal.value)
            {
                // not already defined in this grammar
                let sid = self.next_sid + terminal.id - 3;
                let context = self.resolve_context(&other.contexts[terminal.context]);
                let mut nfa = terminal.nfa.clone_no_finals();
                nfa.states[nfa.exit]
                    .items
                    .push(FinalItem::Terminal(sid, context));
                self.terminals.push(Terminal {
                    id: sid,
                    name: terminal.name.clone(),
                    value: terminal.value.clone(),
                    input_ref: terminal.input_ref,
                    nfa,
                    context,
                    is_fragment: terminal.is_fragment,
                    is_anonymous: terminal.is_anonymous,
                    terminal_references: Vec::new()
                });
            }
        }
    }

    /// Inherits the virtuals from the parent grammar
    fn inherit_virtuals(&mut self, other: &Grammar) {
        for symbol in &other.virtuals {
            self.inherit_virtal(symbol);
        }
    }

    /// Inherits the actions from the parent grammar
    fn inherit_actions(&mut self, other: &Grammar) {
        for symbol in &other.actions {
            self.inherit_action(symbol);
        }
    }

    /// Inherits the variables from the parent grammar
    fn inherit_variables(&mut self, other: &Grammar) {
        for symbol in &other.variables {
            self.inherit_variable(symbol);
        }
    }

    /// Inherits the grammar rules from the parent grammar
    fn inherit_rules(&mut self, other: &Grammar) {
        for variable in &other.variables {
            let head = self
                .variables
                .iter()
                .find(|v| v.name == variable.name)
                .unwrap()
                .id;
            let mut rules = variable
                .rules
                .iter()
                .map(|rule| {
                    let context_name = &other.contexts[rule.context];
                    let context = self
                        .contexts
                        .iter()
                        .position(|c| c == context_name)
                        .unwrap();
                    let elements = rule
                        .body
                        .elements
                        .iter()
                        .map(|element| {
                            RuleBodyElement::new(
                                self.map_symbol_ref(other, element.symbol),
                                element.action,
                                element.input_ref
                            )
                        })
                        .collect();
                    Rule::new(
                        head,
                        rule.head_action,
                        rule.head_input_ref,
                        RuleBody::from_parts(elements),
                        context
                    )
                })
                .collect();
            let head = self
                .variables
                .iter_mut()
                .find(|v| v.name == variable.name)
                .unwrap();
            head.rules.append(&mut rules);
        }
    }

    /// Creates the equivalent template rule symbol for this grammar
    fn inherit_template_rule_symbol(
        &self,
        other: &Grammar,
        symbol: &TemplateRuleSymbol
    ) -> TemplateRuleSymbol {
        match symbol {
            TemplateRuleSymbol::Parameter(index) => TemplateRuleSymbol::Parameter(*index),
            TemplateRuleSymbol::Symbol(symbol) => {
                TemplateRuleSymbol::Symbol(self.map_symbol_ref(other, *symbol))
            }
            TemplateRuleSymbol::Template(template_ref) => {
                let name = &other.template_rules[template_ref.template].name;
                let index = self
                    .template_rules
                    .iter()
                    .enumerate()
                    .find(|(_i, r)| &r.name == name)
                    .unwrap()
                    .0;
                TemplateRuleSymbol::Template(TemplateRuleRef {
                    template: index,
                    input_ref: template_ref.input_ref,
                    arguments: template_ref
                        .arguments
                        .iter()
                        .map(|symbol| self.inherit_template_rule_symbol(other, symbol))
                        .collect()
                })
            }
        }
    }

    /// Inherit the template rules from the parent grammar
    fn inherit_template_rules(&mut self, other: &Grammar) {
        let mut couples: Vec<(usize, &TemplateRule)> = Vec::new();
        for rule in &other.template_rules {
            if self.template_rules.iter().all(|tr| tr.name != rule.name) {
                // does not exist yet
                let index = self.template_rules.len();
                self.template_rules.push(TemplateRule {
                    name: rule.name.clone(),
                    input_ref: rule.input_ref,
                    parameters: rule.parameters.clone(),
                    head_action: rule.head_action,
                    context: rule.context,
                    bodies: Vec::new(),
                    instances: rule
                        .instances
                        .iter()
                        .map(|instance| {
                            let old_variable = other
                                .variables
                                .iter()
                                .find(|v| v.id == instance.head)
                                .unwrap();
                            let new_variable = self
                                .variables
                                .iter()
                                .find(|v| v.name == old_variable.name)
                                .unwrap();
                            TemplateRuleInstance {
                                arguments: instance
                                    .arguments
                                    .iter()
                                    .map(|arg| self.map_symbol_ref(other, *arg))
                                    .collect(),
                                head: new_variable.id
                            }
                        })
                        .collect()
                });
                couples.push((index, rule));
            }
        }

        for (index, other_rule) in couples {
            for body in &other_rule.bodies {
                let mut elements = Vec::new();
                for element in &body.elements {
                    let symbol = self.inherit_template_rule_symbol(other, &element.symbol);
                    elements.push(TemplateRuleElement {
                        symbol,
                        action: element.action,
                        input_ref: element.input_ref
                    });
                }
                self.template_rules[index]
                    .bodies
                    .push(TemplateRuleBody { elements });
            }
        }
    }

    /// Maps a symbol from a grammar to this one
    fn map_symbol_ref(&self, other: &Grammar, symbol: SymbolRef) -> SymbolRef {
        match symbol {
            SymbolRef::Dummy => SymbolRef::Dummy,
            SymbolRef::Epsilon => SymbolRef::Epsilon,
            SymbolRef::Dollar => SymbolRef::Dollar,
            SymbolRef::NullTerminal => SymbolRef::NullTerminal,
            SymbolRef::Terminal(id) => {
                let other_symbol = other.terminals.iter().find(|s| s.id == id).unwrap();
                let symbol = self
                    .terminals
                    .iter()
                    .find(|s| s.name == other_symbol.name)
                    .unwrap();
                SymbolRef::Terminal(symbol.id)
            }
            SymbolRef::Variable(id) => {
                let other_symbol = other.variables.iter().find(|s| s.id == id).unwrap();
                let symbol = self
                    .variables
                    .iter()
                    .find(|s| s.name == other_symbol.name)
                    .unwrap();
                SymbolRef::Variable(symbol.id)
            }
            SymbolRef::Virtual(id) => {
                let other_symbol = other.virtuals.iter().find(|s| s.id == id).unwrap();
                let symbol = self
                    .virtuals
                    .iter()
                    .find(|s| s.name == other_symbol.name)
                    .unwrap();
                SymbolRef::Virtual(symbol.id)
            }
            SymbolRef::Action(id) => {
                let other_symbol = other.actions.iter().find(|s| s.id == id).unwrap();
                let symbol = self
                    .actions
                    .iter()
                    .find(|s| s.name == other_symbol.name)
                    .unwrap();
                SymbolRef::Action(symbol.id)
            }
        }
    }

    /// Builds the complete DFA that matches the terminals in this grammar
    #[allow(clippy::similar_names)]
    #[must_use]
    pub fn build_dfa(&self) -> DFA {
        let mut nfa = NFA::new_minimal();
        for terminal in self.terminals.iter().filter(|t| !t.is_fragment) {
            let (entry, _) = nfa.insert_sub_nfa(&terminal.nfa);
            nfa.add_transition(nfa.entry, EPSILON, entry);
        }
        let mut dfa = DFA::from_nfa(nfa).minimize();
        dfa.repack_transitions();
        dfa.prune();
        dfa
    }

    /// Prepares this grammar for code and data generation
    /// This methods inserts a new grammar rule as its axiom and computes the FIRSTS and FOLLOWERS sets
    ///
    /// # Errors
    ///
    /// Return an error when the axiom is not properly defined
    pub fn prepare(&mut self, grammar_index: usize) -> Result<(), Error> {
        self.add_real_axiom(grammar_index)?;
        for variable in &mut self.variables {
            variable.compute_choices();
        }
        self.compute_firsts();
        self.compute_followers();
        Ok(())
    }

    /// Adds the real axiom to this grammar
    fn add_real_axiom(&mut self, grammar_index: usize) -> Result<(), Error> {
        let axiom_option = self
            .options
            .get(OPTION_AXIOM)
            .ok_or(Error::AxiomNotSpecified(grammar_index))?;
        let axiom_id = self
            .variables
            .iter()
            .find(|v| v.name == axiom_option.value)
            .ok_or(Error::AxiomNotDefined(grammar_index))?
            .id;
        let input_ref = axiom_option.value_input_ref;
        // Create the real axiom rule variable and rule
        let real_axiom = self.add_variable(GENERATED_AXIOM);
        real_axiom.rules.push(Rule::new(
            real_axiom.id,
            TREE_ACTION_NONE,
            input_ref,
            RuleBody::from_parts(vec![
                RuleBodyElement::new(SymbolRef::Variable(axiom_id), TREE_ACTION_PROMOTE, None),
                RuleBodyElement::new(SymbolRef::Dollar, TREE_ACTION_DROP, None),
            ]),
            0
        ));
        Ok(())
    }

    /// Computes the FIRSTS sets for this grammar
    fn compute_firsts(&mut self) {
        let mut firsts_for_var = HashMap::new();
        let mut modified = true;
        while modified {
            modified = false;
            for variable in &mut self.variables {
                modified |= variable.compute_firsts(&mut firsts_for_var);
            }
        }
    }

    /// Computes the FOLLOWERS sets for this grammar
    fn compute_followers(&mut self) {
        let mut followers = HashMap::new();
        // Apply step 1 to each variable
        for variable in &self.variables {
            variable.compute_initial_follower(&mut followers);
        }
        // Apply step 2 and 3 while some modification has occured
        let mut modified = true;
        while modified {
            modified = false;
            for variable in &self.variables {
                modified |= variable.propagate_followers(&mut followers);
            }
        }
        for variable in &mut self.variables {
            if let Some(followers) = followers.remove(&variable.id) {
                variable.followers = followers;
            }
        }
    }

    /// Build data for this grammar
    ///
    /// # Errors
    ///
    /// Return the errors produced when building the grammar
    pub fn build(
        &mut self,
        parsing_method: Option<ParsingMethod>,
        grammar_index: usize
    ) -> Result<BuildData, Vec<Error>> {
        if let Err(error) = self.prepare(grammar_index) {
            return Err(vec![error]);
        };
        // Build DFA
        let dfa = self.build_dfa();
        // Check that no terminal match the empty string
        if !dfa.states.is_empty() && dfa.states[0].is_final() {
            return Err(dfa.states[0]
                .items
                .iter()
                .map(|item| Error::TerminalMatchesEmpty(grammar_index, (*item).into()))
                .collect());
        }
        // Build the data for the lexer
        let expected = dfa.get_expected();
        let separator = match self.get_separator(grammar_index, &expected, &dfa) {
            Ok(separator) => separator,
            Err(error) => return Err(vec![error])
        };
        let method = match self.get_parsing_method(parsing_method, grammar_index) {
            Ok(method) => method,
            Err(error) => return Err(vec![error])
        };
        // Build the data for the parser
        let graph = crate::lr::build_graph(self, grammar_index, &expected, &dfa, method)?;
        Ok(BuildData {
            dfa,
            expected,
            separator,
            method,
            graph
        })
    }

    /// Gets the separator for the grammar
    fn get_separator(
        &self,
        grammar_index: usize,
        expected: &TerminalSet,
        dfa: &DFA
    ) -> Result<Option<TerminalRef>, Error> {
        let Some(option) = self.get_option(OPTION_SEPARATOR) else {
            return Ok(None);
        };
        let Some(terminal) = self.get_terminal_for_name(&option.value) else {
            return Err(Error::SeparatorNotDefined(grammar_index));
        };
        let terminal_ref = TerminalRef::Terminal(terminal.id);
        // warn if the separator is context-sensitive
        if terminal.context != 0 {
            return Err(Error::SeparatorIsContextual(grammar_index, terminal_ref));
        }
        if expected.content.contains(&terminal_ref) {
            // the terminal is produced by the lexer => ok
            return Ok(Some(terminal_ref));
        }
        // the separator will not be produced by the lexer, try to investigate why
        let overriders = dfa.get_overriders(terminal_ref, 0);
        Err(Error::SeparatorCannotBeMatched(
            grammar_index,
            UnmatchableTokenError {
                terminal: terminal_ref,
                overriders
            }
        ))
    }

    /// Gets the parsing method
    fn get_parsing_method(
        &self,
        parsing_method: Option<ParsingMethod>,
        grammar_index: usize
    ) -> Result<ParsingMethod, Error> {
        match parsing_method {
            Some(method) => Ok(method),
            None => match self.get_option(OPTION_METHOD) {
                Some(option) => match option.value.as_ref() {
                    "lr0" => Ok(ParsingMethod::LR0),
                    "lr1" => Ok(ParsingMethod::LR1),
                    "lalr1" => Ok(ParsingMethod::LALR1),
                    "rnglr1" => Ok(ParsingMethod::RNGLR1),
                    "rnglalr1" => Ok(ParsingMethod::RNGLALR1),
                    _ => Err(Error::InvalidOption(
                        grammar_index,
                        OPTION_METHOD.to_string(),
                        vec![
                            String::from("lr0"),
                            String::from("lr1"),
                            String::from("lalr1"),
                            String::from("rnglr1"),
                            String::from("rnglalr1"),
                        ]
                    ))
                },
                None => Ok(ParsingMethod::LALR1)
            }
        }
    }

    /// Builds the in-memory parser for a grammar
    ///
    /// # Errors
    ///
    /// Returns the errors produced by the grammar's compilation
    pub fn get_in_memory<'a>(&'a self, data: &BuildData) -> Result<InMemoryParser<'a>, Vec<Error>> {
        crate::output::build_in_memory_grammar(self, data)
    }
}
