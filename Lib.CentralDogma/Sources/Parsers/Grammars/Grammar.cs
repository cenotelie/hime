/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using Hime.Utils.Reporting;

namespace Hime.Parsers
{
    abstract class Grammar
    {
        protected string name;
        protected ushort nextSID;
        protected Dictionary<string, string> options;
        protected Dictionary<string, Terminal> terminalsByName;
        protected Dictionary<string, Terminal> terminalsByValue;
        protected Dictionary<string, Variable> variables;
        protected Dictionary<string, Virtual> virtuals;
        protected Dictionary<string, Action> actions;

        public string Name { get { return name; } }
        public ICollection<string> Options { get { return options.Keys; } }
        public ICollection<Terminal> Terminals { get { return terminalsByName.Values; } }
        public ICollection<Variable> Variables { get { return variables.Values; } }
        public ICollection<Virtual> Virtuals { get { return virtuals.Values; } }
        public ICollection<Action> Actions { get { return actions.Values; } }
        public List<Rule> Rules
        {
            get
            {
                List<Rule> rules = new List<Rule>();
                foreach (Variable var in variables.Values)
                    foreach (Rule rule in var.Rules)
                        rules.Add(rule);
                return rules;
            }
        }

        public Grammar(string name)
        {
            this.options = new Dictionary<string, string>();
            this.terminalsByName = new Dictionary<string, Terminal>();
            this.terminalsByValue = new Dictionary<string, Terminal>();
            this.variables = new Dictionary<string, Variable>();
            this.virtuals = new Dictionary<string, Virtual>();
            this.actions = new Dictionary<string, Action>();
            this.name = name;
            this.nextSID = 3;
        }

        public void AddOption(string name, string value)
        {
            if (options.ContainsKey(name))
                options[name] = value;
            else
                options.Add(name, value);
        }
		
        public string GetOption(string name)
        {
            return options[name];
        }

        public GrammarSymbol GetSymbol(string name)
        {
            if (terminalsByName.ContainsKey(name)) return terminalsByName[name];
            if (variables.ContainsKey(name)) return variables[name];
            if (virtuals.ContainsKey(name)) return virtuals[name];
            if (actions.ContainsKey(name)) return actions[name];
            return null;
        }

        public TerminalText AddTerminalAnon(string display, Automata.NFA nfa)
        {
            string name = "_t" + nextSID.ToString("X");
            return AddTerminal(name, display, nfa);
        }
        public TerminalText AddTerminalNamed(string name, Automata.NFA nfa) { return AddTerminal(name, name, nfa); }
        private TerminalText AddTerminal(string name, string display, Automata.NFA nfa)
        {
            TerminalText terminal = new TerminalText(nextSID, name, display, nextSID, nfa);
            nextSID++;
            terminalsByName.Add(name, terminal);
            terminalsByValue.Add(display, terminal);
            return terminal;
        }

        public Terminal GetTerminalByName(string name)
        {
            if (!terminalsByName.ContainsKey(name))
                return null;
            return terminalsByName[name];
        }
        public Terminal GetTerminalByValue(string value)
        {
            if (!terminalsByValue.ContainsKey(value))
                return null;
            return terminalsByValue[value];
        }

        public abstract Variable AddVariable(string name);
        public abstract Variable NewVariable();

        public Variable GetVariable(string name)
        {
            if (!variables.ContainsKey(name))
                return null;
            return variables[name];
        }

        public Virtual AddVirtual(string name)
        {
            if (virtuals.ContainsKey(name)) return virtuals[name];
            Virtual Virtual = new Virtual(name);
            virtuals.Add(name, Virtual);
            return Virtual;
        }

        public Virtual GetVirtual(string name)
        {
            if (!virtuals.ContainsKey(name))
                return null;
            return virtuals[name];
        }

        public Action AddAction(string name)
        {
            Action Action = new Action(name);
            actions.Add(name, Action);
            return Action;
        }

        public Action GetAction(string name)
        {
            if (!actions.ContainsKey(name))
                return null;
            return actions[name];
        }

        public abstract Rule CreateRule(Variable head, List<RuleBodyElement> body);

        public abstract void Inherit(Grammar parent);
        
        public Grammar Clone()
        {
            Grammar clone = CreateCopy();
            clone.Inherit(this);
            return clone;
        }

        protected abstract Grammar CreateCopy();

        protected void InheritOptions(Grammar parent)
        {
            foreach (string option in parent.Options)
                AddOption(option, parent.GetOption(option));
        }
		
        protected void InheritActions(Grammar parent)
        {
            foreach (Action action in parent.Actions)
                AddAction(action.Name);
        }
        protected void InheritVirtuals(Grammar parent)
        {
            foreach (Virtual vir in parent.Virtuals)
                AddVirtual(vir.Name);
        }

        public abstract LexerData GetLexerData(Reporter reporter);
        public abstract ParserData GetParserData(Reporter reporter, ParserGenerator generator);
    }
}
