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

//! Error handling

use hime_redist::text::TextPosition;
use miette::{
    Diagnostic, LabeledSpan, MietteError, Severity, SourceCode, SourceOffset, SourceSpan,
    SpanContents
};

use super::{ContextualizedError, Error};
use crate::grammars::{OPTION_AXIOM, OPTION_SEPARATOR};
use crate::lr::LookaheadOrigin;
use crate::{InputReference, LoadedInput};

/// The content for a miette span
struct TextSpanContents<'a> {
    name: &'a str,
    content: &'a str,
    span: SourceSpan,
    position: TextPosition,
    line_count: usize
}

impl<'a> SpanContents<'a> for TextSpanContents<'a> {
    fn name(&self) -> Option<&str> {
        Some(self.name)
    }

    fn data(&self) -> &'a [u8] {
        self.content.as_bytes()
    }

    fn span(&self) -> &miette::SourceSpan {
        &self.span
    }

    fn line(&self) -> usize {
        self.position.line - 1
    }

    fn column(&self) -> usize {
        self.position.column - 1
    }

    fn line_count(&self) -> usize {
        self.line_count
    }
}

impl<'t> LoadedInput<'t> {
    fn get_span_content(
        &self,
        span: SourceSpan,
        context_lines_before: usize,
        context_lines_after: usize
    ) -> TextSpanContents {
        // compute lines
        let original_line = self.content.get_position_at(span.offset()).line;
        let start_line = if context_lines_before != 0 && original_line > 1 {
            let l = original_line.saturating_sub(context_lines_before);
            if l < 1 {
                1
            } else {
                l
            }
        } else {
            original_line
        };
        let end_line = original_line + context_lines_after;

        // compute offsets
        let start_offset = self.content.get_line_index(start_line);
        let end_offset = if end_line - 1 < self.content.get_line_count() {
            self.content.get_line_index(end_line) + self.content.get_line_length(end_line)
        } else {
            self.content.len()
        };
        let length = end_offset - start_offset;

        TextSpanContents {
            name: &self.name,
            content: self.content.get_value(start_offset, length),
            span: SourceSpan::new(SourceOffset::from(start_offset), SourceOffset::from(length)),
            position: TextPosition {
                line: start_line,
                column: 1
            },
            line_count: end_line + 1 - start_line
        }
    }
}

impl<'t> SourceCode for LoadedInput<'t> {
    fn read_span<'a>(
        &'a self,
        span: &SourceSpan,
        context_lines_before: usize,
        context_lines_after: usize
    ) -> Result<Box<dyn SpanContents<'a> + 'a>, MietteError> {
        let contents = self.get_span_content(*span, context_lines_before, context_lines_after);
        Ok(Box::new(contents))
    }
}

impl<'context, 'error, 't> ContextualizedError<'context, 'error, 't> {
    /// Gets the source code associated to a grammar
    fn get_source_code_for_grammar(&self, grammar_index: usize) -> &dyn SourceCode {
        &self.context.inputs[self.context.grammars[grammar_index].input_ref.input_index]
    }

    /// Gets a single miette label without a known location
    fn get_single_label_no_input(&self) -> Box<dyn Iterator<Item = miette::LabeledSpan> + '_> {
        Box::new(std::iter::once(LabeledSpan::new(
            Some(self.to_string()),
            0,
            0
        )))
    }

    /// Gets a single miette label with a known location
    fn get_single_label_with_input(
        &self,
        input: &InputReference
    ) -> Box<dyn Iterator<Item = miette::LabeledSpan> + '_> {
        Box::new(std::iter::once(self.label_for_input(input)))
    }

    /// Gets a single miette label with a known location
    fn get_single_label_with_grammar(
        &self,
        grammar_index: usize
    ) -> Box<dyn Iterator<Item = miette::LabeledSpan> + '_> {
        let input = &self.context.grammars[grammar_index].input_ref;
        self.get_single_label_with_input(input)
    }

    /// Gets a single miette label with a known location
    fn label_for_input(&self, input: &InputReference) -> LabeledSpan {
        self.label_for_input_with_text(input, self.to_string())
    }

    /// Gets a single miette label with a known location
    fn label_for_input_with_text(&self, input: &InputReference, text: String) -> LabeledSpan {
        let offset = self.context.inputs[input.input_index]
            .content
            .get_index_at(input.position);
        LabeledSpan::new(Some(text), offset, input.length)
    }
}

impl<'context, 'error, 't> Diagnostic for ContextualizedError<'context, 'error, 't> {
    fn severity(&self) -> Option<Severity> {
        Some(Severity::Error)
    }

    #[allow(clippy::match_same_arms)]
    fn source_code(&self) -> Option<&dyn SourceCode> {
        match &self.error {
            Error::Io(_) => None,
            Error::Msg(_) => None,
            Error::GrammarNotSpecified => None,
            Error::GrammarNotFound(_) => None,
            Error::Parsing(input, _) => Some(&self.context.inputs[input.input_index]),
            Error::InvalidOption(grammar_index, _name, _valid) => {
                Some(self.get_source_code_for_grammar(*grammar_index))
            }
            Error::AxiomNotSpecified(grammar_index) => {
                Some(self.get_source_code_for_grammar(*grammar_index))
            }
            Error::AxiomNotDefined(grammar_index) => {
                Some(self.get_source_code_for_grammar(*grammar_index))
            }
            Error::SeparatorNotDefined(grammar_index) => {
                Some(self.get_source_code_for_grammar(*grammar_index))
            }
            Error::SeparatorIsContextual(grammar_index, _terminal_ref) => {
                Some(self.get_source_code_for_grammar(*grammar_index))
            }
            Error::SeparatorCannotBeMatched(grammar_index, _error) => {
                Some(self.get_source_code_for_grammar(*grammar_index))
            }
            Error::TemplateRuleNotFound(input, _name) => {
                Some(&self.context.inputs[input.input_index])
            }
            Error::TemplateRuleWrongNumberOfArgs(input, _expected, _provided) => {
                Some(&self.context.inputs[input.input_index])
            }
            Error::SymbolNotFound(input, _name) => Some(&self.context.inputs[input.input_index]),
            Error::InvalidCharacterSpan(input) => Some(&self.context.inputs[input.input_index]),
            Error::UnknownUnicodeBlock(input, _name) => {
                Some(&self.context.inputs[input.input_index])
            }
            Error::UnknownUnicodeCategory(input, _name) => {
                Some(&self.context.inputs[input.input_index])
            }
            Error::UnsupportedNonPlane0InCharacterClass(input, _c) => {
                Some(&self.context.inputs[input.input_index])
            }
            Error::InvalidCodePoint(input, _c) => Some(&self.context.inputs[input.input_index]),
            Error::OverridingPreviousTerminal(input, _name, _previous) => {
                Some(&self.context.inputs[input.input_index])
            }
            Error::GrammarNotDefined(input, _name) => Some(&self.context.inputs[input.input_index]),
            Error::LrConflict(grammar_index, _conflict) => {
                Some(self.get_source_code_for_grammar(*grammar_index))
            }
            Error::TerminalOutsideContext(grammar_index, _error) => {
                Some(self.get_source_code_for_grammar(*grammar_index))
            }
            Error::TerminalCannotBeMatched(grammar_index, _error) => {
                Some(self.get_source_code_for_grammar(*grammar_index))
            }
            Error::TerminalMatchesEmpty(grammar_index, _terminal_ref) => {
                Some(self.get_source_code_for_grammar(*grammar_index))
            }
        }
    }

    #[allow(clippy::match_same_arms, clippy::too_many_lines)]
    fn labels(&self) -> Option<Box<dyn Iterator<Item = miette::LabeledSpan> + '_>> {
        match &self.error {
            Error::Io(_) => Some(self.get_single_label_no_input()),
            Error::Msg(_) => Some(self.get_single_label_no_input()),
            Error::GrammarNotSpecified => Some(self.get_single_label_no_input()),
            Error::GrammarNotFound(_) => Some(self.get_single_label_no_input()),
            Error::Parsing(input, _) => Some(self.get_single_label_with_input(input)),
            Error::InvalidOption(grammar_index, name, _valid) => {
                let option = self.context.grammars[*grammar_index]
                    .get_option(name)
                    .unwrap();
                Some(self.get_single_label_with_input(&option.value_input_ref))
            }
            Error::AxiomNotSpecified(grammar_index) => {
                Some(self.get_single_label_with_grammar(*grammar_index))
            }
            Error::AxiomNotDefined(grammar_index) => {
                let option = self.context.grammars[*grammar_index]
                    .get_option(OPTION_AXIOM)
                    .unwrap();
                Some(self.get_single_label_with_input(&option.value_input_ref))
            }
            Error::SeparatorNotDefined(grammar_index) => {
                let option = self.context.grammars[*grammar_index]
                    .get_option(OPTION_SEPARATOR)
                    .unwrap();
                Some(self.get_single_label_with_input(&option.value_input_ref))
            }
            Error::SeparatorIsContextual(grammar_index, terminal_ref) => {
                let input = &self.context.grammars[*grammar_index]
                    .get_terminal(terminal_ref.sid())
                    .unwrap()
                    .input_ref;
                Some(self.get_single_label_with_input(input))
            }
            Error::SeparatorCannotBeMatched(grammar_index, error) => {
                let grammar = &self.context.grammars[*grammar_index];
                let separator = grammar.get_terminal(error.terminal.sid()).unwrap();
                let mut labels = vec![self.label_for_input(&separator.input_ref)];
                for overrider in &error.overriders {
                    let terminal = grammar.get_terminal(overrider.sid()).unwrap();
                    labels.push(self.label_for_input_with_text(
                        &terminal.input_ref,
                        format!("{} overrides {}", terminal.value, separator.value)
                    ));
                }
                Some(Box::new(labels.into_iter()))
            }
            Error::TemplateRuleNotFound(input, _name) => {
                Some(self.get_single_label_with_input(input))
            }
            Error::TemplateRuleWrongNumberOfArgs(input, _expected, _provided) => {
                Some(self.get_single_label_with_input(input))
            }
            Error::SymbolNotFound(input, _name) => Some(self.get_single_label_with_input(input)),
            Error::InvalidCharacterSpan(input) => Some(self.get_single_label_with_input(input)),
            Error::UnknownUnicodeBlock(input, _name) => {
                Some(self.get_single_label_with_input(input))
            }
            Error::UnknownUnicodeCategory(input, _name) => {
                Some(self.get_single_label_with_input(input))
            }
            Error::UnsupportedNonPlane0InCharacterClass(input, _c) => {
                Some(self.get_single_label_with_input(input))
            }
            Error::InvalidCodePoint(input, _c) => Some(self.get_single_label_with_input(input)),
            Error::OverridingPreviousTerminal(input, name, previous) => Some(Box::new(
                vec![
                    self.label_for_input(input),
                    self.label_for_input_with_text(
                        previous,
                        format!("previous definition of {name}")
                    ),
                ]
                .into_iter()
            )),
            Error::GrammarNotDefined(input, _name) => Some(self.get_single_label_with_input(input)),
            Error::LrConflict(grammar_index, conflict) => {
                let grammar = &self.context.grammars[*grammar_index];
                let mut labels = vec![self.label_for_input(&grammar.input_ref)];
                for item in &conflict.shift_items {
                    let rule = item.rule.get_rule_in(grammar);
                    let choice = &rule.body.choices[0];
                    let value = grammar.get_symbol_value(conflict.lookahead.terminal.into());
                    let input_ref = choice.elements[item.position].input_ref.unwrap();
                    labels.push(self.label_for_input_with_text(
                        &input_ref,
                        format!("Could consume `{value}` at this point")
                    ));
                }
                for item in &conflict.reduce_items {
                    let rule = item.rule.get_rule_in(grammar);
                    let choice = &rule.body.choices[0];
                    let lookahead = item.lookaheads.get(conflict.lookahead.terminal).unwrap();
                    let value = grammar.get_symbol_value(conflict.lookahead.terminal.into());
                    if choice.elements.is_empty() {
                        // do not display this choice
                    } else if item.position >= choice.elements.len() {
                        let input_ref = choice.elements[choice.elements.len() - 1]
                            .input_ref
                            .unwrap();
                        labels.push(self.label_for_input_with_text(
                            &input_ref,
                            format!(
                                "Could match the rule ending here when looking ahead to `{value}`"
                            )
                        ));
                    } else {
                        let input_ref = choice.elements[item.position].input_ref.unwrap();
                        labels.push(self.label_for_input_with_text(
                            &input_ref,
                            format!(
                                "Could match the rule ending here when looking ahead to `{value}`"
                            )
                        ));
                    }
                    for origin in &lookahead.origins {
                        let LookaheadOrigin::FirstOf(choice_ref) = origin;
                        let rule = choice_ref.rule.get_rule_in(grammar);
                        let choice = &rule.body.choices[0];
                        if let Some(input_ref) = choice.elements[choice_ref.position].input_ref {
                            labels.push(self.label_for_input_with_text(
                                &input_ref,
                                format!("`{value}` can be expected, looking from here")
                            ));
                        }
                    }
                }
                Some(Box::new(labels.into_iter()))
            }
            Error::TerminalOutsideContext(grammar_index, error) => {
                let grammar = &self.context.grammars[*grammar_index];
                let mut labels = vec![self.label_for_input(&grammar.input_ref)];
                for item in &error.items {
                    let rule = item.rule.get_rule_in(grammar);
                    let choice = &rule.body.choices[0];
                    let input_ref = choice.elements[item.position].input_ref.unwrap();
                    labels.push(self.label_for_input_with_text(
                        &input_ref,
                        String::from("Used outside required context")
                    ));
                }
                Some(Box::new(labels.into_iter()))
            }
            Error::TerminalCannotBeMatched(grammar_index, error) => {
                let grammar = &self.context.grammars[*grammar_index];
                let separator = grammar.get_terminal(error.terminal.sid()).unwrap();
                let mut labels = vec![self.label_for_input(&separator.input_ref)];
                for overrider in &error.overriders {
                    let terminal = grammar.get_terminal(overrider.sid()).unwrap();
                    labels.push(self.label_for_input_with_text(
                        &terminal.input_ref,
                        format!("{} overrides {}", terminal.value, separator.value)
                    ));
                }
                Some(Box::new(labels.into_iter()))
            }
            Error::TerminalMatchesEmpty(grammar_index, terminal_ref) => {
                let input = &self.context.grammars[*grammar_index]
                    .get_terminal(terminal_ref.sid())
                    .unwrap()
                    .input_ref;
                Some(self.get_single_label_with_input(input))
            }
        }
    }

    fn help<'s>(&'s self) -> Option<Box<dyn std::fmt::Display + 's>> {
        match &self.error {
            Error::InvalidOption(_grammar_index, _name, valid) => {
                if valid.is_empty() {
                    None
                } else {
                    Some(Box::new(format!("expected one of: {}", valid.join(", "))))
                }
            }
            Error::LrConflict(grammar_index, conflict) => {
                if conflict.phrases.is_empty() {
                    None
                } else {
                    let grammar = &self.context.grammars[*grammar_index];
                    Some(Box::new(format!(
                        "Example of input that is ambiguous: {}",
                        conflict.phrases[0]
                            .0
                            .iter()
                            .map(|s| grammar.get_symbol_value((*s).into()))
                            .collect::<Vec<_>>()
                            .join(" ")
                    )))
                }
            }
            Error::TerminalOutsideContext(grammar_index, error) => {
                if error.phrases.is_empty() {
                    None
                } else {
                    let grammar = &self.context.grammars[*grammar_index];
                    Some(Box::new(format!(
                        "Example of input that poses this problem: {}",
                        error.phrases[0]
                            .0
                            .iter()
                            .map(|s| grammar.get_symbol_value((*s).into()))
                            .collect::<Vec<_>>()
                            .join(" ")
                    )))
                }
            }
            _ => None
        }
    }
}
