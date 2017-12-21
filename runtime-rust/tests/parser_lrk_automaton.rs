/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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

extern crate hime_redist;

use hime_redist::parsers::LRkAutomaton;

/// Static resource for the serialized parser automaton
const PARSER_AUTOMATON: &'static [u8] = include_bytes!("HimeGrammarParser.bin");

#[test]
fn test_loaded() {
    let automaton = LRkAutomaton::new(PARSER_AUTOMATON);
    assert_eq!(automaton.get_states_count(), 174);
}
