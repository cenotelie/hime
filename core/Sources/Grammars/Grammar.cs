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
using Hime.Redist.Utils;

namespace Hime.SDK.Grammars
{
	/// <summary>
	/// Represents a grammar
	/// </summary>
	public class Grammar
	{
		/// <summary>
		/// The prefix for the generated terminal names
		/// </summary>
		public const string PREFIX_GENERATED_TERMINAL = "_gen_T";
		/// <summary>
		/// The prefix for the generated variable names
		/// </summary>
		public const string PREFIX_GENERATED_VARIABLE = "_gen_V";
		/// <summary>
		/// The name of the generated axiom variable
		/// </summary>
		public const string GENERATED_AXIOM = "_Axiom_";
		/// <summary>
		/// Name of the grammar option specifying the grammar's axiom variable
		/// </summary>
		public const string OPTION_AXIOM = "Axiom";
		/// <summary>
		/// Name of the grammar option specifying the grammar's separator terminal
		/// </summary>
		public const string OPTION_SEPARATOR = "Separator";
		/// <summary>
		/// The name of the default lexical context
		/// </summary>
		public const string DEFAULT_CONTEXT_NAME = "_default_";

		/// <summary>
		/// The counter for the generation of unique names across multiple grammars
		/// </summary>
		private static int unique;

		/// <summary>
		/// The grammar's name
		/// </summary>
		private readonly string name;
		/// <summary>
		/// The next unique symbol identifier for this grammar
		/// </summary>
		private int nextSID;
		/// <summary>
		/// The grammar's options
		/// </summary>
		private readonly Dictionary<string, string> options;
		/// <summary>
		/// The lexical contexts defined in this grammar
		/// </summary>
		private readonly List<string> contexts;
		/// <summary>
		/// The fragments of terminals (used in the definition of complete terminals)
		/// </summary>
		private readonly Dictionary<string, Terminal> fragments;
		/// <summary>
		/// The grammar's terminals, by name
		/// </summary>
		private readonly Dictionary<string, Terminal> terminalsByName;
		/// <summary>
		/// The grammar's terminals, by value
		/// </summary>
		private readonly Dictionary<string, Terminal> terminalsByValue;
		/// <summary>
		/// The grammar's variables
		/// </summary>
		private readonly Dictionary<string, Variable> variables;
		/// <summary>
		/// The grammar's virtual symbols
		/// </summary>
		private readonly Dictionary<string, Virtual> virtuals;
		/// <summary>
		/// The grammar's action symbols
		/// </summary>
		private readonly Dictionary<string, Action> actions;
		/// <summary>
		/// The grammar's template rules
		/// </summary>
		private readonly List<TemplateRule> templateRules;

		/// <summary>
		/// Gets the grammar's name
		/// </summary>
		public string Name { get { return name; } }

		/// <summary>
		/// Gets the grammar's options
		/// </summary>
		public ICollection<string> Options { get { return options.Keys; } }

		/// <summary>
		/// Gets the contexts defined in this grammar
		/// </summary>
		public ROList<string> Contexts { get { return new ROList<string>(contexts); } }

		/// <summary>
		/// Gets the terminal fragments in this grammar
		/// </summary>
		public ICollection<Terminal> Fragments { get { return fragments.Values; } }

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
			options = new Dictionary<string, string>();
			contexts = new List<string>();
			contexts.Add(DEFAULT_CONTEXT_NAME);
			fragments = new Dictionary<string, Terminal>();
			terminalsByName = new Dictionary<string, Terminal>();
			terminalsByValue = new Dictionary<string, Terminal>();
			variables = new Dictionary<string, Variable>();
			virtuals = new Dictionary<string, Virtual>();
			actions = new Dictionary<string, Action>();
			templateRules = new List<TemplateRule>();
			this.name = name;
			nextSID = 3;
		}

		/// <summary>
		/// Generates a unique identifier
		/// </summary>
		/// <returns>A unique identifier</returns>
		private static string GenerateID()
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
			return options.ContainsKey(name) ? options[name] : null;
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
		/// Resolves the specified lexical context name for this grammar
		/// </summary>
		/// <param name="context">The name of a lexical context</param>
		/// <returns>The identifier of the resolved lexical context</returns>
		public int ResolveContext(string context)
		{
			int index = contexts.IndexOf(context);
			if (index >= 0)
				return index;
			contexts.Add(context);
			return contexts.Count - 1;
		}

		/// <summary>
		/// Adds the given fragment to this grammar
		/// </summary>
		/// <param name="name">The fragment's name</param>
		/// <param name="nfa">The fragment's NFA</param>
		/// <returns>The new fragment</returns>
		public Terminal AddFragment(string name, Automata.NFA nfa)
		{
			Terminal fragment = new Terminal(nextSID, name, name, nfa, 0);
			nextSID++;
			fragments.Add(name, fragment);
			return fragment;
		}

		/// <summary>
		/// Gets the fragment with the given name
		/// </summary>
		/// <param name="name">A fragment's name</param>
		/// <returns>The corresponding fragment, or <c>null</c> if it does not exists</returns>
		public Terminal GetFragment(string name)
		{
			return fragments.ContainsKey(name) ? fragments[name] : null;
		}

		/// <summary>
		/// Adds the given anonymous terminal to this grammar
		/// </summary>
		/// <param name="value">The terminal's value</param>
		/// <param name="nfa">The terminal's NFA</param>
		/// <returns>The new terminal</returns>
		public Terminal AddTerminalAnon(string value, Automata.NFA nfa)
		{
			string tName = PREFIX_GENERATED_TERMINAL + GenerateID();
			return AddTerminal(tName, value, nfa, DEFAULT_CONTEXT_NAME);
		}

		/// <summary>
		/// Adds the given named terminal to this grammar
		/// </summary>
		/// <param name="name">The terminal's name</param>
		/// <param name="nfa">The terminal's NFA</param>
		/// <param name="context">The terminal's context</param>
		/// <returns>The new terminal</returns>
		public Terminal AddTerminalNamed(string name, Automata.NFA nfa, string context)
		{
			return AddTerminal(name, name, nfa, context);
		}

		/// <summary>
		/// Adds the given terminal to this grammar
		/// </summary>
		/// <param name="tName">The terminal's name</param>
		/// <param name="value">The terminal's value</param>
		/// <param name="nfa">The terminal's NFA</param>
		/// <param name="context">The terminal's context</param>
		/// <returns>The new terminal</returns>
		private Terminal AddTerminal(string tName, string value, Automata.NFA nfa, string context)
		{
			Terminal terminal = new Terminal(nextSID, tName, value, nfa, ResolveContext(context));
			nextSID++;
			terminalsByName.Add(tName, terminal);
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
			return !terminalsByName.ContainsKey(name) ? null : terminalsByName[name];
		}

		/// <summary>
		/// Gets the terminal with the given value
		/// </summary>
		/// <param name="value">A terminal's value</param>
		/// <returns>The corresponding terminal, or <c>null</c> if it does not exists</returns>
		public Terminal GetTerminalByValue(string value)
		{
			return !terminalsByValue.ContainsKey(value) ? null : terminalsByValue[value];
		}

		/// <summary>
		/// Generates a new variable
		/// </summary>
		/// <returns>A new variable</returns>
		public Variable GenerateVariable()
		{
			return AddVariable(PREFIX_GENERATED_VARIABLE + nextSID);
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
			return !variables.ContainsKey(name) ? null : variables[name];
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
			return !virtuals.ContainsKey(name) ? null : virtuals[name];
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
			return !actions.ContainsKey(name) ? null : actions[name];
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
			bool doClone = (nextSID == 3);
			InheritOptions(parent);
			InheritFragments(parent, doClone);
			InheritTerminals(parent, doClone);
			InheritVirtuals(parent, doClone);
			InheritActions(parent, doClone);
			InheritVariables(parent, doClone);
			InheritTemplateRules(parent);
			if (doClone)
			{
				foreach (Terminal terminal in terminalsByName.Values)
					if (terminal.ID > nextSID)
						nextSID = terminal.ID;
				foreach (Variable variable in variables.Values)
					if (variable.ID > nextSID)
						nextSID = variable.ID;
				foreach (Virtual virt in virtuals.Values)
					if (virt.ID > nextSID)
						nextSID = virt.ID;
				foreach (Action action in actions.Values)
					if (action.ID > nextSID)
						nextSID = action.ID;
				nextSID += 1;
			}
		}

		/// <summary>
		/// Inherits the options from the parent grammar
		/// </summary>
		/// <param name="parent">The parent's grammar</param>
		private void InheritOptions(Grammar parent)
		{
			foreach (string option in parent.Options)
				AddOption(option, parent.GetOption(option));
		}

		/// <summary>
		/// Inherits the fragments from the parent grammar
		/// </summary>
		/// <param name="parent">The parent's grammar</param>
		/// <param name="doClone">Clone the symbols</param>
		private void InheritFragments(Grammar parent, bool doClone)
		{
			List<Terminal> inherited = new List<Terminal>(parent.fragments.Values);
			inherited.Sort(new Terminal.PriorityComparer());
			foreach (Terminal fragment in inherited)
			{
				if (fragments.ContainsKey(fragment.Name))
				{
					// this is a redefinition of a named terminal
					// TODO: Output an error in the log
				}
				else if (doClone)
				{
					Terminal clone = new Terminal(fragment.ID, fragment.Name, fragment.Value, fragment.NFA.Clone(false), fragment.Context);
					clone.NFA.StateExit.AddItem(clone);
					fragments.Add(fragment.Name, fragment);
				}
				else
				{
					Terminal clone = AddFragment(fragment.Name, fragment.NFA.Clone(false));
					clone.NFA.StateExit.AddItem(clone);
				}
			}
		}

		/// <summary>
		/// Inherits the terminals from the parent grammar
		/// </summary>
		/// <param name="parent">The parent's grammar</param>
		/// <param name="doClone">Clone the symbols</param>
		private void InheritTerminals(Grammar parent, bool doClone)
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
				else if (doClone)
				{
					Terminal clone = new Terminal(terminal.ID, terminal.Name, terminal.Value, terminal.NFA.Clone(false), terminal.Context);
					clone.NFA.StateExit.AddItem(clone);
					terminalsByName.Add(clone.Name, terminal);
					terminalsByValue.Add(clone.Value, terminal);
				}
				else
				{
					Terminal clone = AddTerminal(terminal.Name, terminal.Value, terminal.NFA.Clone(false), parent.contexts[terminal.Context]);
					clone.NFA.StateExit.AddItem(clone);
				}
			}
		}

		/// <summary>
		/// Inherits the variables from the parent grammar
		/// </summary>
		/// <param name="parent">The parent's grammar</param>
		/// <param name="doClone">Clone the symbols</param>
		private void InheritVariables(Grammar parent, bool doClone)
		{
			foreach (Variable variable in parent.Variables)
			{
				if (doClone)
				{
					Variable clone = new Variable(variable.ID, variable.Name);
					variables.Add(clone.Name, clone);
				}
				else
				{
					AddVariable(variable.Name);
				}
			}
			foreach (Variable variable in parent.Variables)
			{
				Variable clone = variables[variable.Name];
				foreach (Rule rule in variable.Rules)
				{
					List<RuleBodyElement> parts = new List<RuleBodyElement>();
					for (int i = 0; i != rule.Body.Length; i++)
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
					clone.AddRule(new Rule(clone, new RuleBody(parts), rule.IsGenerated, ResolveContext(parent.contexts[rule.Context])));
				}
			}
		}

		/// <summary>
		/// Inherits the virtuals from the parent grammar
		/// </summary>
		/// <param name="parent">The parent's grammar</param>
		/// <param name="doClone">Clone the symbols</param>
		private void InheritVirtuals(Grammar parent, bool doClone)
		{
			foreach (Virtual vir in parent.Virtuals)
			{
				if (doClone)
				{
					Virtual clone = new Virtual(vir.ID, vir.Name);
					virtuals.Add(clone.Name, clone);
				}
				else
				{
					if (!virtuals.ContainsKey(vir.Name))
						AddVirtual(vir.Name);
				}
			}
		}

		/// <summary>
		/// Inherits the actions from the parent grammar
		/// </summary>
		/// <param name="parent">The parent's grammar</param>
		/// <param name="doClone">Clone the symbols</param>
		private void InheritActions(Grammar parent, bool doClone)
		{
			foreach (Action action in parent.Actions)
			{
				if (doClone)
				{
					Action clone = new Action(action.ID, action.Name);
					actions.Add(clone.Name, clone);
				}
				else
				{
					if (!actions.ContainsKey(action.Name))
						AddAction(action.Name);
				}
			}
		}

		/// <summary>
		/// Inherits the template rules from the parent grammar
		/// </summary>
		/// <param name="parent">The parent's grammar</param>
		private void InheritTemplateRules(Grammar parent)
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
				final.StateEntry.AddTransition(Automata.NFA.EPSILON, sub.StateEntry);
			}
			// Construct the equivalent DFA and minimize it
			Automata.DFA finalDFA = new Automata.DFA(final);
			finalDFA = finalDFA.Minimize();
			finalDFA.RepackTransitions();
			finalDFA.Prune();
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
			if (!options.ContainsKey(OPTION_AXIOM))
				return "No axiom variable has been defined for grammar " + name;
			// Search for the variable specified as the Axiom
			string axiomName = options[OPTION_AXIOM];
			if (!variables.ContainsKey(axiomName))
				return "The specified axiom variable " + axiomName + " is undefined";

			// Create the real axiom rule variable and rule
			Variable axiom = AddVariable(GENERATED_AXIOM);
			List<RuleBodyElement> parts = new List<RuleBodyElement>();
			parts.Add(new RuleBodyElement(variables[axiomName], Hime.Redist.TreeAction.Promote));
			parts.Add(new RuleBodyElement(Dollar.Instance, Hime.Redist.TreeAction.Drop));
			axiom.AddRule(new Rule(axiom, new RuleBody(parts), false, 0));
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
					mod |= var.ComputeFirsts();
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
				var.ComputeInitialFollowers();
			// Apply step 2 and 3 while some modification has occured
			while (mod)
			{
				mod = false;
				foreach (Variable var in variables.Values)
					mod |= var.PropagateFollowers();
			}
		}
	}
}
