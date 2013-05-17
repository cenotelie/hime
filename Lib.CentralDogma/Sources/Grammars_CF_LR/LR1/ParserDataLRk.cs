using System;
using System.IO;
using System.Collections.Generic;
using Hime.Redist.Parsers;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class ParserDataLRk : ParserDataLR
    {
        public ParserDataLRk(Reporting.Reporter reporter, CFGrammar gram, Graph graph) : base(reporter, gram, graph) { }

		protected override string BaseClassName { get { return "LRkParser"; } }

        protected override void ExportAutomaton(StreamWriter stream, string name, string resource)
        {
            stream.WriteLine("        private static readonly LRkAutomaton automaton = LRkAutomaton.FindAutomaton(typeof(" + name + "Parser), \"" + resource + "\");");
        }

        public override void ExportData(BinaryWriter stream)
        {
            List<Rule> rules = new List<Rule>(grammar.Rules);
            stream.Write((ushort)(terminals.Count + variables.Count));  // Nb of columns
            stream.Write((ushort)graph.States.Count);                   // Nb or rows
            stream.Write((ushort)rules.Count);                          // Nb or rules

            foreach (Terminal t in terminals)
                stream.Write(t.SID);
            foreach (Variable var in variables)
                stream.Write(var.SID);

            foreach (State state in graph.States)
                ExportDataTable(stream, state);

            foreach (Rule rule in rules)
                ExportDataProduction(stream, rule);
        }

        private void ExportDataTable(BinaryWriter stream, State state)
        {
            Dictionary<Terminal, Rule> reductions = new Dictionary<Terminal, Rule>();
            foreach (StateActionReduce reduction in state.Reductions)
                reductions.Add(reduction.Lookahead, reduction.ToReduceRule);
            if (reductions.ContainsKey(Epsilon.Instance) || reductions.ContainsKey(NullTerminal.Instance))
                stream.Write((ushort)LRActionCode.Accept);
            else
                stream.Write((ushort)LRActionCode.None);
            stream.Write((ushort)LRActionCode.None);
            for (int i = 1; i != terminals.Count; i++)
            {
                Terminal t = terminals[i];
                if (state.Children.ContainsKey(t))
                {
                    stream.Write((ushort)LRActionCode.Shift);
                    stream.Write((ushort)state.Children[t].ID);
                }
                else if (reductions.ContainsKey(t))
                {
                    stream.Write((ushort)LRActionCode.Reduce);
                    stream.Write((ushort)rules.IndexOf(reductions[t]));
                }
                else if (reductions.ContainsKey(NullTerminal.Instance))
                {
                    stream.Write((ushort)LRActionCode.Reduce);
                    stream.Write((ushort)rules.IndexOf(reductions[NullTerminal.Instance]));
                }
                else
                {
                    stream.Write((ushort)LRActionCode.None);
                    stream.Write((ushort)LRActionCode.None);
                }
            }
            foreach (Variable var in variables)
            {
                if (state.Children.ContainsKey(var))
                {
                    stream.Write((ushort)LRActionCode.Shift);
                    stream.Write((ushort)state.Children[var].ID);
                }
                else
                {
                    stream.Write((ushort)LRActionCode.None);
                    stream.Write((ushort)LRActionCode.None);
                }
            }
        }
    }
}
