using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars
{
    abstract class Grammar
    {
        private static int unique = 0;

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

        protected string GenerateID() { return (unique++).ToString("X"); }

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

        public bool HasOption(string name)
        {
            return options.ContainsKey(name);
        }

        public Symbol GetSymbol(string name)
        {
            if (terminalsByName.ContainsKey(name)) return terminalsByName[name];
            if (variables.ContainsKey(name)) return variables[name];
            if (virtuals.ContainsKey(name)) return virtuals[name];
            if (actions.ContainsKey(name)) return actions[name];
            return null;
        }

        public TextTerminal AddTerminalAnon(string value, Automata.NFA nfa)
        {
            string name = "_t" + GenerateID();
            return AddTerminal(name, value, nfa);
        }
        public TextTerminal AddTerminalNamed(string name, Automata.NFA nfa) { return AddTerminal(name, name, nfa); }
        private TextTerminal AddTerminal(string name, string value, Automata.NFA nfa)
        {
            TextTerminal terminal = new TextTerminal(nextSID, name, value, nextSID, nfa);
            nextSID++;
            terminalsByName.Add(name, terminal);
            terminalsByValue.Add(value, terminal);
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

        protected void InheritOptions(Grammar parent)
        {
            foreach (string option in parent.Options)
                AddOption(option, parent.GetOption(option));
        }

        protected void InheritTerminals(Grammar parent)
        {
            List<Terminal> inherited = new List<Terminal>(parent.terminalsByName.Values);
            inherited.Sort(Terminal.PriorityComparer.Instance);
            foreach (TextTerminal terminal in inherited)
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
                    TextTerminal clone = AddTerminal(terminal.Name, terminal.Value, terminal.NFA.Clone(false));
                    clone.NFA.StateExit.Item = clone;
                }
            }
        }

        protected void InheritActions(Grammar parent)
        {
            foreach (Action action in parent.Actions)
            {
                if (!actions.ContainsKey(action.Name))
                    AddAction(action.Name);
            }
        }
        protected void InheritVirtuals(Grammar parent)
        {
            foreach (Virtual vir in parent.Virtuals)
            {
                if (!virtuals.ContainsKey(vir.Name))
                    AddVirtual(vir.Name);
            }
        }

        public abstract LexerData GetLexerData(Reporting.Reporter reporter);
        public abstract ParserData GetParserData(Reporting.Reporter reporter, ParserGenerator generator);
    }
}
