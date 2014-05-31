/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
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
*
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/
using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars
{
	/// <summary>
	/// Represents a grammar
	/// </summary>
	public class Grammar
	{
		/// <summary>
		/// The prefix for the generated terminal names
		/// </summary>
		public const string prefixGeneratedTerminal = "_gen_T";
		/// <summary>
		/// The prefix for the generated variable names
		/// </summary>
		public const string prefixGeneratedVariable = "_gen_V";
		/// <summary>
		/// The name of the generated axiom variable
		/// </summary>
		public const string generatedAxiom = "_Axiom_";
		/// <summary>
		/// Name of the grammar option specifying the grammar's axiom variable
		/// </summary>
		public const string optionAxiom = "Axiom";
		/// <summary>
		/// Name of the grammar option specifying the grammar's separator terminal
		/// </summary>
		public const string optionSeparator = "Separator";

		/// <summary>
		/// The counter for the generation of unique names across multiple grammars
		/// </summary>
		private static int unique = 0;

		/// <summary>
		/// The grammar's name
		/// </summary>
		private string name;
		/// <summary>
		/// The next unique symbol identifier for this grammar
		/// </summary>
		private int nextSID;
		/// <summary>
		/// The grammar's options
		/// </summary>
		private Dictionary<string, string> options;
		/// <summary>
		/// The grammar's terminals, by name
		/// </summary>
		private Dictionary<string, Terminal> terminalsByName;
		/// <summary>
		/// The grammar's terminals, by value
		/// </summary>
		private Dictionary<string, Terminal> terminalsByValue;
		/// <summary>
		/// The grammar's variables
		/// </summary>
		private Dictionary<string, Variable> variables;
		/// <summary>
		/// The grammar's virtual symbols
		/// </summary>
		private Dictionary<string, Virtual> virtuals;
		/// <summary>
		/// The grammar's action symbols
		/// </summary>
		private Dictionary<string, Action> actions;
		/// <summary>
		/// The grammar's template rules
		/// </summary>
		private List<TemplateRule> templateRules;

		/// <summary>
		/// Gets the grammar's name
		/// </summary>
		public string Name { get { return name; } }
		/// <summary>
		/// Gets the grammar's options
		/// </summary>
		public ICollection<string> Options { get { return options.Keys; } }
		/// <summary>
		/// Gets the grammar's terminals
		/// </summary>
		public ICollection<Terminal> Terminals { get { return terminalsByName.Values; } }
		/// <summary>
		/// Gets teh grammar's variables
		/// </summary>
		public ICollection<Variable> Variables { get { return variables.Values; } }
		/// <summary>
		/// Gets the grammar's virtual symbols
		/// </summary>
		public ICollection<Virtual> Virtuals { get { return virtuals.Values; } }
		/// <summary>
		/// Gets the grammar's action symbols
		/// </summary>
		public ICollection<Action> Actions { get { return actions.Values; } }
		/// <summary>
		/// Gets a list of all the rules in this grammar
		/// </summary>
		public List<Rule> Rules
		{
			get
			{
				List<Rule> rules = new List<Rule>();
				foreach (Variable var in variables.Values)
					rules.AddRange(var.Rules);
				return rules;
			}
		}
		/// <summary>
		/// Gets the template rules in this grammar
		/// </summary>
		public ROList<TemplateRule> TemplateRules { get { return new ROList<TemplateRule>(templateRules); } }

		/// <summary>
		/// Initializes this grammar
		/// </summary>
		/// <param name="name">The grammar's name</param>
		public Grammar(string name)
		{
			this.options = new Dictionary<string, string>();
			this.terminalsByName = new Dictionary<string, Terminal>();
			this.terminalsByValue = new Dictionary<string, Terminal>();
			this.variables = new Dictionary<string, Variable>();
			this.virtuals = new Dictionary<string, Virtual>();
			this.actions = new Dictionary<string, Action>();
			this.templateRules = new List<TemplateRule>();
			this.name = name;
			this.nextSID = 3;
		}

		/// <summary>
		/// Generates a unique identifier
		/// </summary>
		/// <returns>A unique identifier</returns>
		protected string GenerateID()
		{
			return (unique++).ToString("X4");
		}

		/// <summary>
		/// Adds this grammar an option
		/// </summary>
		/// <param name="name">The option's name</param>
		/// <param name="value">The option's value</param>
		public void AddOption(string name, string value)
		{
			if (options.ContainsKey(name))
				options[name] = value;
			else
				options.Add(name, value);
		}

		/// <summary>
		/// Gets the value of the given option
		/// </summary>
		/// <param name="name">An option's name</param>
		/// <returns>The option's value, or <c>null</c> if the option is not set</returns>
		public string GetOption(string name)
		{
			if (options.ContainsKey(name))
				return options[name];
			return null;
		}

		/// <summary>
		/// Gets the symbol with the given name in this grammar
		/// </summary>
		/// <param name="name">A symbol's name</param>
		/// <returns>The corresponding symbol, or <c>null</c> if it does not exists</returns>
		public Symbol GetSymbol(string name)
		{
			if (terminalsByName.ContainsKey(name))
				return terminalsByName[name];
			if (variables.ContainsKey(name))
				return variables[name];
			if (virtuals.ContainsKey(name))
				return virtuals[name];
			if (actions.ContainsKey(name))
				return actions[name];
			return null;
		}

		/// <summary>
		/// Adds the given anonymous terminal to this grammar
		/// </summary>
		/// <param name="value">The terminal's value</param>
		/// <param name="nfa">The terminal's NFA</param>
		/// <returns>The new terminal</returns>
		public Terminal AddTerminalAnon(string value, Automata.NFA nfa)
		{
			string name = prefixGeneratedTerminal + GenerateID();
			return AddTerminal(name, value, nfa);
		}

		/// <summary>
		/// Adds the given named terminal to this grammar
		/// </summary>
		/// <param name="name">The terminal's name</param>
		/// <param name="nfa">The terminal's NFA</param>
		/// <returns>The new terminal</returns>
		public Terminal AddTerminalNamed(string name, Automata.NFA nfa)
		{
			return AddTerminal(name, name, nfa);
		}

		/// <summary>
		/// Adds the given terminal to this grammar
		/// </summary>
		/// <param name="name">The terminal's name</param>
		/// <param name="value">The terminal's value</param>
		/// <param name="nfa">The terminal's NFA</param>
		/// <returns>The new terminal</returns>
		private Terminal AddTerminal(string name, string value, Automata.NFA nfa)
		{
			Terminal terminal = new Terminal(nextSID, name, value, nfa);
			nextSID++;
			terminalsByName.Add(name, terminal);
			terminalsByValue.Add(value, terminal);
			return terminal;
		}

		/// <summary>
		/// Gets the terminal with the given name
		/// </summary>
		/// <param name="name">A terminal's name</param>
		/// <returns>The corresponding terminal, or <c>null</c> if it does not exists</returns>
		public Terminal GetTerminalByName(string name)
		{
			if (!terminalsByName.ContainsKey(name))
				return null;
			return terminalsByName[name];
		}

		/// <summary>
		/// Gets the terminal with the given value
		/// </summary>
		/// <param name="value">A terminal's value</param>
		/// <returns>The corresponding terminal, or <c>null</c> if it does not exists</returns>
		public Terminal GetTerminalByValue(string value)
		{
			if (!terminalsByValue.ContainsKey(value))
				return null;
			return terminalsByValue[value];
		}

		/// <summary>
		/// Generates a new variable
		/// </summary>
		/// <returns>A new variable</returns>
		public Variable GenerateVariable()
		{
			string name = prefixGeneratedVariable + nextSID;
			return AddVariable(name);
		}

		/// <summary>
		/// Adds a variable with the given name to this grammar
		/// </summary>
		/// <param name="name">The variable's name</param>
		/// <returns>The new variable</returns>
		public Variable AddVariable(string name)
		{
			if (variables.ContainsKey(name))
				return variables[name];
			Variable variable = new Variable(nextSID, name);
			nextSID++;
			variables.Add(name, variable);
			return variable;
		}

		/// <summary>
		/// Gets the variable with the given name
		/// </summary>
		/// <param name="name">A variable's name</param>
		/// <returns>The corresponding variable, or <c>null</c> if it does not exists</returns>
		public Variable GetVariable(string name)
		{
			if (!variables.ContainsKey(name))
				return null;
			return variables[name];
		}

		/// <summary>
		/// Adds a virtual symbol with the given name to this grammar
		/// </summary>
		/// <param name="name">The virtual's name</param>
		/// <returns>The new virtual</returns>
		public Virtual AddVirtual(string name)
		{
			if (virtuals.ContainsKey(name))
				return virtuals[name];
			Virtual symbol = new Virtual(nextSID, name);
			nextSID++;
			virtuals.Add(name, symbol);
			return symbol;
		}

		/// <summary>
		/// Gets the virtual symbol with the given name
		/// </summary>
		/// <param name="name">A virtual's name</param>
		/// <returns>The corresponding virtual, or <c>null</c> if it does not exists</returns>
		public Virtual GetVirtual(string name)
		{
			if (!virtuals.ContainsKey(name))
				return null;
			return virtuals[name];
		}

		/// <summary>
		/// Adds an action symbol with the given name to this grammar
		/// </summary>
		/// <param name="name">The action's name</param>
		/// <returns>The new action</returns>
		public Action AddAction(string name)
		{
			if (actions.ContainsKey(name))
				return actions[name];
			Action action = new Action(nextSID, name);
			nextSID++;
			actions.Add(name, action);
			return action;
		}

		/// <summary>
		/// Gets the action symbol with the given name
		/// </summary>
		/// <param name="name">An action's name</param>
		/// <returns>The corresponding action, or <c>null</c> if it does not exists</returns>
		public Action GetAction(string name)
		{
			if (!actions.ContainsKey(name))
				return null;
			return actions[name];
		}

		/// <summary>
		/// Adds the template rule to this grammar
		/// </summary>
		/// <param name="node">The rule's definition AST</param>
		/// <returns>The new template rule</returns>
		public TemplateRule AddTemplateRule(Hime.Redist.ASTNode node)
		{
			TemplateRule rule = new TemplateRule(this, node);
			templateRules.Add(rule);
			return rule;
		}

		/// <summary>
		/// Inherit from the given parent
		/// </summary>
		/// <param name="parent">The parent's grammar</param>
		public void Inherit(Grammar parent)
		{
			InheritOptions(parent);
			InheritTerminals(parent);
			InheritVirtuals(parent);
			InheritActions(parent);
		}

		/// <summary>
		/// Inherits the options from the parent grammar
		/// </summary>
		/// <param name="parent">The parent's grammar</param>
		protected void InheritOptions(Grammar parent)
		{
			foreach (string option in parent.Options)
				AddOption(option, parent.GetOption(option));
		}

		/// <summary>
		/// Inherits the terminals from the parent grammar
		/// </summary>
		/// <param name="parent">The parent's grammar</param>
		protected void InheritTerminals(Grammar parent)
		{
			List<Terminal> inherited = new List<Terminal>(parent.terminalsByName.Values);
			inherited.Sort(new Terminal.PriorityComparer());
			foreach (Terminal terminal in inherited)
			{
				if (terminalsByName.ContainsKey(terminal.Name))
				{
					// this is a redefinition of a named terminal
					// TODO: Output an error in the log
				}
				else if (terminalsByValue.ContainsKey(terminal.Value))
				{
					// this is a redefinition of an inline terminal
					// => do nothing, simply reuse the one with the same value
				}
				else
				{
					Terminal clone = AddTerminal(terminal.Name, terminal.Value, terminal.NFA.Clone(false));
					clone.NFA.StateExit.Item = clone;
				}
			}
		}

		/// <summary>
		/// Inherits the variables from the parent grammar
		/// </summary>
		/// <param name="parent">The parent's grammar</param>
		protected void InheritVariables(Grammar parent)
		{
			foreach (Variable variable in parent.Variables)
				AddVariable(variable.Name);
			foreach (Variable variable in parent.Variables)
			{
				Variable clone = variables[variable.Name];
				foreach (Rule rule in variable.Rules)
				{
					List<RuleBodyElement> parts = new List<RuleBodyElement>();
					for (int i=0; i!=rule.Body.Length; i++)
					{
						RuleBodyElement part = rule.Body[i];
						Symbol symbol = null;
						if (part.Symbol is Variable)
							symbol = variables[part.Symbol.Name];
						else if (part.Symbol is Terminal)
							symbol = terminalsByName[part.Symbol.Name];
						else if (part.Symbol is Virtual)
							symbol = virtuals[part.Symbol.Name];
						else if (part.Symbol is Action)
							symbol = actions[part.Symbol.Name];
						parts.Add(new RuleBodyElement(symbol, part.Action));
					}
					clone.AddRule(new Rule(clone, new RuleBody(parts), rule.IsGenerated));
				}
			}
		}

		/// <summary>
		/// Inherits the virtuals from the parent grammar
		/// </summary>
		/// <param name="parent">The parent's grammar</param>
		protected void InheritVirtuals(Grammar parent)
		{
			foreach (Virtual vir in parent.Virtuals)
			{
				if (!virtuals.ContainsKey(vir.Name))
					AddVirtual(vir.Name);
			}
		}

		/// <summary>
		/// Inherits the actions from the parent grammar
		/// </summary>
		/// <param name="parent">The parent's grammar</param>
		protected void InheritActions(Grammar parent)
		{
			foreach (Action action in parent.Actions)
			{
				if (!actions.ContainsKey(action.Name))
					AddAction(action.Name);
			}
		}

		/// <summary>
		/// Inherits the template rules from the parent grammar
		/// </summary>
		/// <param name="parent">The parent's grammar</param>
		protected void InheritTemplateRules(Grammar parent)
		{
			foreach (TemplateRule tRule in parent.TemplateRules)
				templateRules.Add(new TemplateRule(tRule, this));
		}

		/// <summary>
		/// Builds the complete DFA that matches the terminals in this grammar
		/// </summary>
		/// <returns>The DFA</returns>
		public Automata.DFA BuildDFA()
		{
			// Construct a global NFA for all the terminals
			Automata.NFA final = Automata.NFA.NewMinimal();
			foreach (Terminal terminal in terminalsByName.Values)
			{
				Automata.NFA sub = terminal.NFA.Clone();
				final.InsertSubNFA(sub);
				final.StateEntry.AddTransition(Automata.NFA.Epsilon, sub.StateEntry);
			}
			// Construct the equivalent DFA and minimize it
			Automata.DFA finalDFA = new Automata.DFA(final);
			finalDFA = finalDFA.Minimize();
			finalDFA.RepackTransitions();
			return finalDFA;
		}

		/// <summary>
		/// Prepares this grammar for code and data generation
		/// </summary>
		/// <returns>The error message, or <c>null</c> if all went well</returns>
		/// <remarks>
		/// This methods inserts a new grammar rule as its axiom and computes the FIRSTS and FOLLOWERS sets
		/// </remarks>
		public string Prepare()
		{
			string message = AddRealAxiom();
			if (message != null)
				return message;
			ComputeFirsts();
			ComputeFollowers();
			return null;
		}

		/// <summary>
		/// Adds the real axiom to this grammar
		/// </summary>
		/// <returns>An error message, if any</returns>
		private string AddRealAxiom()
		{
			// Search for Axiom option
			if (!options.ContainsKey(optionAxiom))
				return "No axiom variable has been defined for grammar " + this.name;
			// Search for the variable specified as the Axiom
			string name = options[optionAxiom];
			if (!variables.ContainsKey(name))
				return "The specified axiom variable " + name + " is undefined";

			// Create the real axiom rule variable and rule
			Variable axiom = AddVariable(generatedAxiom);
			List<RuleBodyElement> parts = new List<RuleBodyElement>();
			parts.Add(new RuleBodyElement(variables[name], Hime.Redist.TreeAction.Promote));
			parts.Add(new RuleBodyElement(Dollar.Instance, Hime.Redist.TreeAction.Drop));
			axiom.AddRule(new Rule(axiom, new RuleBody(parts), false));
			return null;
		}

		/// <summary>
		/// Computes the FIRSTS sets for this grammar
		/// </summary>
		private void ComputeFirsts()
		{
			bool mod = true;
			// While some modification has occured, repeat the process
			while (mod)
			{
				mod = false;
				foreach (Variable var in variables.Values)
					if (var.ComputeFirsts())
						mod = true;
			}
		}

		/// <summary>
		/// Computes the FOLLOWERS sets for this grammar
		/// </summary>
		private void ComputeFollowers()
		{
			bool mod = true;
			// Apply step 1 to each variable
			foreach (Variable var in variables.Values)
				var.ComputeFollowers_Step1();
			// Apply step 2 and 3 while some modification has occured
			while (mod)
			{
				mod = false;
				foreach (Variable var in variables.Values)
					if (var.ComputeFollowers_Step23())
						mod = true;
			}
		}
	}
}
