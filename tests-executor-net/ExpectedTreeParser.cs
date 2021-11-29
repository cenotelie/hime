/*
 * WARNING: this file has been generated by
 * Hime Parser Generator 3.5.2
 */

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using Hime.Redist;
using Hime.Redist.Parsers;

namespace Hime.Tests.Executor
{
	/// <summary>
	/// Represents a parser
	/// </summary>
	[GeneratedCodeAttribute("Hime.SDK", "3.5.2")]
	internal class ExpectedTreeParser : LRkParser
	{
		/// <summary>
		/// The automaton for this parser
		/// </summary>
		private static readonly LRkAutomaton commonAutomaton = LRkAutomaton.Find(typeof(ExpectedTreeParser), "ExpectedTreeParser.bin");
		/// <summary>
		/// Contains the constant IDs for the variables and virtuals in this parser
		/// </summary>
		[GeneratedCodeAttribute("Hime.SDK", "3.5.2")]
		public class ID
		{
			/// <summary>
			/// The unique identifier for variable option
			/// </summary>
			public const int VariableOption = 0x001F;
			/// <summary>
			/// The unique identifier for variable terminal_def_atom
			/// </summary>
			public const int VariableTerminalDefAtom = 0x0020;
			/// <summary>
			/// The unique identifier for variable terminal_def_element
			/// </summary>
			public const int VariableTerminalDefElement = 0x0021;
			/// <summary>
			/// The unique identifier for variable terminal_def_cardinalilty
			/// </summary>
			public const int VariableTerminalDefCardinalilty = 0x0022;
			/// <summary>
			/// The unique identifier for variable terminal_def_repetition
			/// </summary>
			public const int VariableTerminalDefRepetition = 0x0023;
			/// <summary>
			/// The unique identifier for variable terminal_def_fragment
			/// </summary>
			public const int VariableTerminalDefFragment = 0x0024;
			/// <summary>
			/// The unique identifier for variable terminal_def_restrict
			/// </summary>
			public const int VariableTerminalDefRestrict = 0x0025;
			/// <summary>
			/// The unique identifier for variable terminal_definition
			/// </summary>
			public const int VariableTerminalDefinition = 0x0026;
			/// <summary>
			/// The unique identifier for variable terminal_rule
			/// </summary>
			public const int VariableTerminalRule = 0x0027;
			/// <summary>
			/// The unique identifier for variable terminal_fragment
			/// </summary>
			public const int VariableTerminalFragment = 0x0028;
			/// <summary>
			/// The unique identifier for variable terminal_context
			/// </summary>
			public const int VariableTerminalContext = 0x0029;
			/// <summary>
			/// The unique identifier for variable terminal_item
			/// </summary>
			public const int VariableTerminalItem = 0x002A;
			/// <summary>
			/// The unique identifier for variable rule_sym_action
			/// </summary>
			public const int VariableRuleSymAction = 0x002B;
			/// <summary>
			/// The unique identifier for variable rule_sym_virtual
			/// </summary>
			public const int VariableRuleSymVirtual = 0x002C;
			/// <summary>
			/// The unique identifier for variable rule_sym_ref_params
			/// </summary>
			public const int VariableRuleSymRefParams = 0x002D;
			/// <summary>
			/// The unique identifier for variable rule_sym_ref_template
			/// </summary>
			public const int VariableRuleSymRefTemplate = 0x002E;
			/// <summary>
			/// The unique identifier for variable rule_sym_ref_simple
			/// </summary>
			public const int VariableRuleSymRefSimple = 0x002F;
			/// <summary>
			/// The unique identifier for variable rule_def_atom
			/// </summary>
			public const int VariableRuleDefAtom = 0x0030;
			/// <summary>
			/// The unique identifier for variable rule_def_context
			/// </summary>
			public const int VariableRuleDefContext = 0x0031;
			/// <summary>
			/// The unique identifier for variable rule_def_sub
			/// </summary>
			public const int VariableRuleDefSub = 0x0032;
			/// <summary>
			/// The unique identifier for variable rule_def_element
			/// </summary>
			public const int VariableRuleDefElement = 0x0033;
			/// <summary>
			/// The unique identifier for variable rule_def_tree_action
			/// </summary>
			public const int VariableRuleDefTreeAction = 0x0034;
			/// <summary>
			/// The unique identifier for variable rule_def_repetition
			/// </summary>
			public const int VariableRuleDefRepetition = 0x0035;
			/// <summary>
			/// The unique identifier for variable rule_def_fragment
			/// </summary>
			public const int VariableRuleDefFragment = 0x0036;
			/// <summary>
			/// The unique identifier for variable rule_def_choice
			/// </summary>
			public const int VariableRuleDefChoice = 0x0037;
			/// <summary>
			/// The unique identifier for variable rule_definition
			/// </summary>
			public const int VariableRuleDefinition = 0x0038;
			/// <summary>
			/// The unique identifier for variable rule_template_params
			/// </summary>
			public const int VariableRuleTemplateParams = 0x0039;
			/// <summary>
			/// The unique identifier for variable cf_rule_template
			/// </summary>
			public const int VariableCfRuleTemplate = 0x003A;
			/// <summary>
			/// The unique identifier for variable cf_rule_simple
			/// </summary>
			public const int VariableCfRuleSimple = 0x003B;
			/// <summary>
			/// The unique identifier for variable cf_rule
			/// </summary>
			public const int VariableCfRule = 0x003C;
			/// <summary>
			/// The unique identifier for variable grammar_options
			/// </summary>
			public const int VariableGrammarOptions = 0x003D;
			/// <summary>
			/// The unique identifier for variable grammar_terminals
			/// </summary>
			public const int VariableGrammarTerminals = 0x003E;
			/// <summary>
			/// The unique identifier for variable grammar_cf_rules
			/// </summary>
			public const int VariableGrammarCfRules = 0x003F;
			/// <summary>
			/// The unique identifier for variable grammar_parency
			/// </summary>
			public const int VariableGrammarParency = 0x0040;
			/// <summary>
			/// The unique identifier for variable cf_grammar
			/// </summary>
			public const int VariableCfGrammar = 0x0041;
			/// <summary>
			/// The unique identifier for variable file
			/// </summary>
			public const int VariableFile = 0x0042;
			/// <summary>
			/// The unique identifier for variable fixture
			/// </summary>
			public const int VariableFixture = 0x0063;
			/// <summary>
			/// The unique identifier for variable header
			/// </summary>
			public const int VariableHeader = 0x0064;
			/// <summary>
			/// The unique identifier for variable test
			/// </summary>
			public const int VariableTest = 0x0065;
			/// <summary>
			/// The unique identifier for variable test_matches
			/// </summary>
			public const int VariableTestMatches = 0x0066;
			/// <summary>
			/// The unique identifier for variable test_no_match
			/// </summary>
			public const int VariableTestNoMatch = 0x0067;
			/// <summary>
			/// The unique identifier for variable test_fails
			/// </summary>
			public const int VariableTestFails = 0x0068;
			/// <summary>
			/// The unique identifier for variable test_output
			/// </summary>
			public const int VariableTestOutput = 0x0069;
			/// <summary>
			/// The unique identifier for variable tree
			/// </summary>
			public const int VariableTree = 0x006A;
			/// <summary>
			/// The unique identifier for variable check
			/// </summary>
			public const int VariableCheck = 0x006B;
			/// <summary>
			/// The unique identifier for variable children
			/// </summary>
			public const int VariableChildren = 0x006C;
			/// <summary>
			/// The unique identifier for virtual range
			/// </summary>
			public const int VirtualRange = 0x0047;
			/// <summary>
			/// The unique identifier for virtual concat
			/// </summary>
			public const int VirtualConcat = 0x004B;
			/// <summary>
			/// The unique identifier for virtual emptypart
			/// </summary>
			public const int VirtualEmptypart = 0x0058;
		}
		/// <summary>
		/// The collection of variables matched by this parser
		/// </summary>
		/// <remarks>
		/// The variables are in an order consistent with the automaton,
		/// so that variable indices in the automaton can be used to retrieve the variables in this table
		/// </remarks>
		private static readonly Symbol[] variables = {
			new Symbol(0x001F, "option"), 
			new Symbol(0x0020, "terminal_def_atom"), 
			new Symbol(0x0021, "terminal_def_element"), 
			new Symbol(0x0022, "terminal_def_cardinalilty"), 
			new Symbol(0x0023, "terminal_def_repetition"), 
			new Symbol(0x0024, "terminal_def_fragment"), 
			new Symbol(0x0025, "terminal_def_restrict"), 
			new Symbol(0x0026, "terminal_definition"), 
			new Symbol(0x0027, "terminal_rule"), 
			new Symbol(0x0028, "terminal_fragment"), 
			new Symbol(0x0029, "terminal_context"), 
			new Symbol(0x002A, "terminal_item"), 
			new Symbol(0x002B, "rule_sym_action"), 
			new Symbol(0x002C, "rule_sym_virtual"), 
			new Symbol(0x002D, "rule_sym_ref_params"), 
			new Symbol(0x002E, "rule_sym_ref_template"), 
			new Symbol(0x002F, "rule_sym_ref_simple"), 
			new Symbol(0x0030, "rule_def_atom"), 
			new Symbol(0x0031, "rule_def_context"), 
			new Symbol(0x0032, "rule_def_sub"), 
			new Symbol(0x0033, "rule_def_element"), 
			new Symbol(0x0034, "rule_def_tree_action"), 
			new Symbol(0x0035, "rule_def_repetition"), 
			new Symbol(0x0036, "rule_def_fragment"), 
			new Symbol(0x0037, "rule_def_choice"), 
			new Symbol(0x0038, "rule_definition"), 
			new Symbol(0x0039, "rule_template_params"), 
			new Symbol(0x003A, "cf_rule_template"), 
			new Symbol(0x003B, "cf_rule_simple"), 
			new Symbol(0x003C, "cf_rule"), 
			new Symbol(0x003D, "grammar_options"), 
			new Symbol(0x003E, "grammar_terminals"), 
			new Symbol(0x003F, "grammar_cf_rules"), 
			new Symbol(0x0040, "grammar_parency"), 
			new Symbol(0x0041, "cf_grammar"), 
			new Symbol(0x0042, "file"), 
			new Symbol(0x004C, "__V76"), 
			new Symbol(0x004D, "__V77"), 
			new Symbol(0x004E, "__V78"), 
			new Symbol(0x0051, "__V81"), 
			new Symbol(0x0054, "__V84"), 
			new Symbol(0x0057, "__V87"), 
			new Symbol(0x0059, "__V89"), 
			new Symbol(0x005A, "__V90"), 
			new Symbol(0x005B, "__V91"), 
			new Symbol(0x005C, "__V92"), 
			new Symbol(0x005D, "__V93"), 
			new Symbol(0x005F, "__V95"), 
			new Symbol(0x0061, "__V97"), 
			new Symbol(0x0063, "fixture"), 
			new Symbol(0x0064, "header"), 
			new Symbol(0x0065, "test"), 
			new Symbol(0x0066, "test_matches"), 
			new Symbol(0x0067, "test_no_match"), 
			new Symbol(0x0068, "test_fails"), 
			new Symbol(0x0069, "test_output"), 
			new Symbol(0x006A, "tree"), 
			new Symbol(0x006B, "check"), 
			new Symbol(0x006C, "children"), 
			new Symbol(0x006D, "__V109"), 
			new Symbol(0x0076, "__V118"), 
			new Symbol(0x0077, "__V119"), 
			new Symbol(0x0079, "__V121"), 
			new Symbol(0x007A, "__VAxiom") };
		/// <summary>
		/// The collection of virtuals matched by this parser
		/// </summary>
		/// <remarks>
		/// The virtuals are in an order consistent with the automaton,
		/// so that virtual indices in the automaton can be used to retrieve the virtuals in this table
		/// </remarks>
		private static readonly Symbol[] virtuals = {
			new Symbol(0x0047, "range"), 
			new Symbol(0x004B, "concat"), 
			new Symbol(0x0058, "emptypart") };
		/// <summary>
		/// Initializes a new instance of the parser
		/// </summary>
		/// <param name="lexer">The input lexer</param>
		public ExpectedTreeParser(ExpectedTreeLexer lexer) : base (commonAutomaton, variables, virtuals, null, lexer) { }

		/// <summary>
		/// Visitor interface
		/// </summary>
		[GeneratedCodeAttribute("Hime.SDK", "3.5.2")]
		public class Visitor
		{
			public virtual void OnTerminalSeparator(ASTNode node) {}
			public virtual void OnTerminalName(ASTNode node) {}
			public virtual void OnTerminalInteger(ASTNode node) {}
			public virtual void OnTerminalLiteralString(ASTNode node) {}
			public virtual void OnTerminalLiteralAny(ASTNode node) {}
			public virtual void OnTerminalLiteralText(ASTNode node) {}
			public virtual void OnTerminalLiteralClass(ASTNode node) {}
			public virtual void OnTerminalUnicodeBlock(ASTNode node) {}
			public virtual void OnTerminalUnicodeCategory(ASTNode node) {}
			public virtual void OnTerminalUnicodeCodepoint(ASTNode node) {}
			public virtual void OnTerminalUnicodeSpanMarker(ASTNode node) {}
			public virtual void OnTerminalOperatorOptional(ASTNode node) {}
			public virtual void OnTerminalOperatorZeromore(ASTNode node) {}
			public virtual void OnTerminalOperatorOnemore(ASTNode node) {}
			public virtual void OnTerminalOperatorUnion(ASTNode node) {}
			public virtual void OnTerminalOperatorDifference(ASTNode node) {}
			public virtual void OnTerminalTreeActionPromote(ASTNode node) {}
			public virtual void OnTerminalTreeActionDrop(ASTNode node) {}
			public virtual void OnTerminalBlockOptions(ASTNode node) {}
			public virtual void OnTerminalBlockTerminals(ASTNode node) {}
			public virtual void OnTerminalBlockRules(ASTNode node) {}
			public virtual void OnTerminalBlockContext(ASTNode node) {}
			public virtual void OnTerminalNodeName(ASTNode node) {}
			public virtual void OnVariableOption(ASTNode node) {}
			public virtual void OnVariableTerminalDefAtom(ASTNode node) {}
			public virtual void OnVariableTerminalDefElement(ASTNode node) {}
			public virtual void OnVariableTerminalDefCardinalilty(ASTNode node) {}
			public virtual void OnVariableTerminalDefRepetition(ASTNode node) {}
			public virtual void OnVariableTerminalDefFragment(ASTNode node) {}
			public virtual void OnVariableTerminalDefRestrict(ASTNode node) {}
			public virtual void OnVariableTerminalDefinition(ASTNode node) {}
			public virtual void OnVariableTerminalRule(ASTNode node) {}
			public virtual void OnVariableTerminalFragment(ASTNode node) {}
			public virtual void OnVariableTerminalContext(ASTNode node) {}
			public virtual void OnVariableTerminalItem(ASTNode node) {}
			public virtual void OnVariableRuleSymAction(ASTNode node) {}
			public virtual void OnVariableRuleSymVirtual(ASTNode node) {}
			public virtual void OnVariableRuleSymRefParams(ASTNode node) {}
			public virtual void OnVariableRuleSymRefTemplate(ASTNode node) {}
			public virtual void OnVariableRuleSymRefSimple(ASTNode node) {}
			public virtual void OnVariableRuleDefAtom(ASTNode node) {}
			public virtual void OnVariableRuleDefContext(ASTNode node) {}
			public virtual void OnVariableRuleDefSub(ASTNode node) {}
			public virtual void OnVariableRuleDefElement(ASTNode node) {}
			public virtual void OnVariableRuleDefTreeAction(ASTNode node) {}
			public virtual void OnVariableRuleDefRepetition(ASTNode node) {}
			public virtual void OnVariableRuleDefFragment(ASTNode node) {}
			public virtual void OnVariableRuleDefChoice(ASTNode node) {}
			public virtual void OnVariableRuleDefinition(ASTNode node) {}
			public virtual void OnVariableRuleTemplateParams(ASTNode node) {}
			public virtual void OnVariableCfRuleTemplate(ASTNode node) {}
			public virtual void OnVariableCfRuleSimple(ASTNode node) {}
			public virtual void OnVariableCfRule(ASTNode node) {}
			public virtual void OnVariableGrammarOptions(ASTNode node) {}
			public virtual void OnVariableGrammarTerminals(ASTNode node) {}
			public virtual void OnVariableGrammarCfRules(ASTNode node) {}
			public virtual void OnVariableGrammarParency(ASTNode node) {}
			public virtual void OnVariableCfGrammar(ASTNode node) {}
			public virtual void OnVariableFile(ASTNode node) {}
			public virtual void OnVariableFixture(ASTNode node) {}
			public virtual void OnVariableHeader(ASTNode node) {}
			public virtual void OnVariableTest(ASTNode node) {}
			public virtual void OnVariableTestMatches(ASTNode node) {}
			public virtual void OnVariableTestNoMatch(ASTNode node) {}
			public virtual void OnVariableTestFails(ASTNode node) {}
			public virtual void OnVariableTestOutput(ASTNode node) {}
			public virtual void OnVariableTree(ASTNode node) {}
			public virtual void OnVariableCheck(ASTNode node) {}
			public virtual void OnVariableChildren(ASTNode node) {}
			public virtual void OnVirtualRange(ASTNode node) {}
			public virtual void OnVirtualConcat(ASTNode node) {}
			public virtual void OnVirtualEmptypart(ASTNode node) {}
		}

		/// <summary>
		/// Walk the AST of a result using a visitor
		/// <param name="result">The parse result</param>
		/// <param name="visitor">The visitor to use</param>
		/// </summary>
		public static void Visit(ParseResult result, Visitor visitor)
		{
			VisitASTNode(result.Root, visitor);
		}

		/// <summary>
		/// Walk the sub-AST from the specified node using a visitor
		/// </summary>
		/// <param name="node">The AST node to start from</param>
		/// <param name="visitor">The visitor to use</param>
		public static void VisitASTNode(ASTNode node, Visitor visitor)
		{
			for (int i = 0; i < node.Children.Count; i++)
				VisitASTNode(node.Children[i], visitor);
			switch(node.Symbol.ID)
			{
				case 0x0007: visitor.OnTerminalSeparator(node); break;
				case 0x0009: visitor.OnTerminalName(node); break;
				case 0x000A: visitor.OnTerminalInteger(node); break;
				case 0x000C: visitor.OnTerminalLiteralString(node); break;
				case 0x000D: visitor.OnTerminalLiteralAny(node); break;
				case 0x000E: visitor.OnTerminalLiteralText(node); break;
				case 0x000F: visitor.OnTerminalLiteralClass(node); break;
				case 0x0010: visitor.OnTerminalUnicodeBlock(node); break;
				case 0x0011: visitor.OnTerminalUnicodeCategory(node); break;
				case 0x0012: visitor.OnTerminalUnicodeCodepoint(node); break;
				case 0x0013: visitor.OnTerminalUnicodeSpanMarker(node); break;
				case 0x0014: visitor.OnTerminalOperatorOptional(node); break;
				case 0x0015: visitor.OnTerminalOperatorZeromore(node); break;
				case 0x0016: visitor.OnTerminalOperatorOnemore(node); break;
				case 0x0017: visitor.OnTerminalOperatorUnion(node); break;
				case 0x0018: visitor.OnTerminalOperatorDifference(node); break;
				case 0x0019: visitor.OnTerminalTreeActionPromote(node); break;
				case 0x001A: visitor.OnTerminalTreeActionDrop(node); break;
				case 0x001B: visitor.OnTerminalBlockOptions(node); break;
				case 0x001C: visitor.OnTerminalBlockTerminals(node); break;
				case 0x001D: visitor.OnTerminalBlockRules(node); break;
				case 0x001E: visitor.OnTerminalBlockContext(node); break;
				case 0x0062: visitor.OnTerminalNodeName(node); break;
				case 0x001F: visitor.OnVariableOption(node); break;
				case 0x0020: visitor.OnVariableTerminalDefAtom(node); break;
				case 0x0021: visitor.OnVariableTerminalDefElement(node); break;
				case 0x0022: visitor.OnVariableTerminalDefCardinalilty(node); break;
				case 0x0023: visitor.OnVariableTerminalDefRepetition(node); break;
				case 0x0024: visitor.OnVariableTerminalDefFragment(node); break;
				case 0x0025: visitor.OnVariableTerminalDefRestrict(node); break;
				case 0x0026: visitor.OnVariableTerminalDefinition(node); break;
				case 0x0027: visitor.OnVariableTerminalRule(node); break;
				case 0x0028: visitor.OnVariableTerminalFragment(node); break;
				case 0x0029: visitor.OnVariableTerminalContext(node); break;
				case 0x002A: visitor.OnVariableTerminalItem(node); break;
				case 0x002B: visitor.OnVariableRuleSymAction(node); break;
				case 0x002C: visitor.OnVariableRuleSymVirtual(node); break;
				case 0x002D: visitor.OnVariableRuleSymRefParams(node); break;
				case 0x002E: visitor.OnVariableRuleSymRefTemplate(node); break;
				case 0x002F: visitor.OnVariableRuleSymRefSimple(node); break;
				case 0x0030: visitor.OnVariableRuleDefAtom(node); break;
				case 0x0031: visitor.OnVariableRuleDefContext(node); break;
				case 0x0032: visitor.OnVariableRuleDefSub(node); break;
				case 0x0033: visitor.OnVariableRuleDefElement(node); break;
				case 0x0034: visitor.OnVariableRuleDefTreeAction(node); break;
				case 0x0035: visitor.OnVariableRuleDefRepetition(node); break;
				case 0x0036: visitor.OnVariableRuleDefFragment(node); break;
				case 0x0037: visitor.OnVariableRuleDefChoice(node); break;
				case 0x0038: visitor.OnVariableRuleDefinition(node); break;
				case 0x0039: visitor.OnVariableRuleTemplateParams(node); break;
				case 0x003A: visitor.OnVariableCfRuleTemplate(node); break;
				case 0x003B: visitor.OnVariableCfRuleSimple(node); break;
				case 0x003C: visitor.OnVariableCfRule(node); break;
				case 0x003D: visitor.OnVariableGrammarOptions(node); break;
				case 0x003E: visitor.OnVariableGrammarTerminals(node); break;
				case 0x003F: visitor.OnVariableGrammarCfRules(node); break;
				case 0x0040: visitor.OnVariableGrammarParency(node); break;
				case 0x0041: visitor.OnVariableCfGrammar(node); break;
				case 0x0042: visitor.OnVariableFile(node); break;
				case 0x0063: visitor.OnVariableFixture(node); break;
				case 0x0064: visitor.OnVariableHeader(node); break;
				case 0x0065: visitor.OnVariableTest(node); break;
				case 0x0066: visitor.OnVariableTestMatches(node); break;
				case 0x0067: visitor.OnVariableTestNoMatch(node); break;
				case 0x0068: visitor.OnVariableTestFails(node); break;
				case 0x0069: visitor.OnVariableTestOutput(node); break;
				case 0x006A: visitor.OnVariableTree(node); break;
				case 0x006B: visitor.OnVariableCheck(node); break;
				case 0x006C: visitor.OnVariableChildren(node); break;
				case 0x0047: visitor.OnVirtualRange(node); break;
				case 0x004B: visitor.OnVirtualConcat(node); break;
				case 0x0058: visitor.OnVirtualEmptypart(node); break;
			}
		}
	}
}
