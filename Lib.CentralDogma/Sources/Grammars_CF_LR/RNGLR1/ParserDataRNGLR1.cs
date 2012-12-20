/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System;
using System.IO;
using System.Collections.Generic;
using Hime.Redist.Parsers;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class ParserDataRNGLR1 : ParserDataLR
    {
        public ParserDataRNGLR1(Reporting.Reporter reporter, CFGrammar gram, Graph graph) : base(reporter, gram, graph) { }

		protected override string BaseClassName { get { return "RNGLRParser"; } }

        protected override void ExportAutomaton(StreamWriter stream, string name, string resource)
        {
            stream.WriteLine("        private static readonly RNGLRAutomaton automaton = RNGLRAutomaton.FindAutomaton(typeof(" + name + "Parser), \"" + resource + "\");");
        }

        public override void ExportData(BinaryWriter stream)
        {
            List<KeyValuePair<Rule, int>> rules = new List<KeyValuePair<Rule, int>>();
            List<ushort> nullables = new List<ushort>();
            foreach (CFRule rule in grammar.Rules)
            {
                rules.Add(new KeyValuePair<Rule, int>(rule, rule.CFBody.GetChoiceAt(0).Length));
                if (rule.CFBody.GetChoiceAt(0).Firsts.Contains(Epsilon.Instance))
                    nullables.Add((ushort)(rules.Count - 1));
                for (int i = 1; i < rule.CFBody.GetChoiceAt(0).Length; i++)
                {
                    if (rule.CFBody.GetChoiceAt(i).Firsts.Contains(Epsilon.Instance))
                    {
                        rules.Add(new KeyValuePair<Rule, int>(rule, i));
                        nullables.Add((ushort)(rules.Count - 1));
                    }
                }
            }

            int total = 0;
            List<ushort> offsets = new List<ushort>();
            List<ushort> counts = new List<ushort>();
            foreach (State state in graph.States)
                total = ExportDataCountActions(offsets, counts, total, state);

            stream.Write((ushort)(terminals.Count + variables.Count));  // Nb of columns
            stream.Write((ushort)graph.States.Count);                   // Nb or rows
            stream.Write((ushort)total);                                // Nb of actions
            stream.Write((ushort)rules.Count);                          // Nb of rules
            stream.Write((ushort)nullables.Count);                      // Nb of nullables

            foreach (Terminal t in terminals)
                stream.Write(t.SID);
            foreach (Variable var in variables)
                stream.Write(var.SID);

            for (int i = 0; i != offsets.Count; i++)
            {
                stream.Write(counts[i]);
                stream.Write(offsets[i]);
            }

            foreach (State state in graph.States)
                ExportDataTable(stream, rules, state);

            foreach (KeyValuePair<Rule, int> pair in rules)
                ExportDataProduction(stream, pair.Key, pair.Value);

            foreach (ushort index in nullables)
                stream.Write(index);
        }

        private int ExportDataCountActions(List<ushort> offsets, List<ushort> counts, int total, State state)
        {
            Dictionary<Terminal, int> counters = new Dictionary<Terminal, int>();
            foreach (StateActionReduce reduce in state.Reductions)
            {
                if (counters.ContainsKey(reduce.Lookahead))
                    counters.Add(reduce.Lookahead, counters[reduce.Lookahead] + 1);
                else
                    counters.Add(reduce.Lookahead, 1);
            }
            foreach (Terminal t in terminals)
            {
                int count = state.Children.Count;
                if (counters.ContainsKey(t))
                    count += counters[t];
                offsets.Add((ushort)total);
                counts.Add((ushort)count);
                total += count;
            }
            foreach (Variable var in variables)
            {
                int count = state.Children.ContainsKey(var) ? 1 : 0;
                offsets.Add((ushort)total);
                counts.Add((ushort)count);
                total += count;
            }
            return total;
        }

        private void ExportDataTable(BinaryWriter stream, List<KeyValuePair<Rule, int>> rules, State state)
        {
            Dictionary<Terminal, List<StateActionRNReduce>> reductions = new Dictionary<Terminal, List<StateActionRNReduce>>();
            foreach (StateActionReduce reduce in state.Reductions)
            {
                if (!reductions.ContainsKey(reduce.Lookahead))
                    reductions.Add(reduce.Lookahead, new List<StateActionRNReduce>());
                reductions[reduce.Lookahead].Add(reduce as StateActionRNReduce);
            }
            if (reductions.ContainsKey(Epsilon.Instance))
            {
                // There can be only one reduction on epsilon
                stream.Write(LRkAutomaton.ActionAccept);
                stream.Write(LRkAutomaton.ActionNone);
            }
            for (int i = 1; i != terminals.Count; i++)
            {
                Terminal t = terminals[i];
                if (state.Children.ContainsKey(t))
                {
                    stream.Write(LRkAutomaton.ActionShift);
                    stream.Write((ushort)state.Children[t].ID);
                }
                if (reductions.ContainsKey(t))
                {
                    foreach (StateActionRNReduce reduce in reductions[t])
                    {
                        stream.Write(LRkAutomaton.ActionReduce);
                        stream.Write((ushort)rules.IndexOf(new KeyValuePair<Rule, int>(reduce.ToReduceRule, reduce.ReduceLength)));
                    }
                }
            }
            foreach (Variable var in variables)
            {
                if (state.Children.ContainsKey(var))
                {
                    stream.Write(LRkAutomaton.ActionShift);
                    stream.Write((ushort)state.Children[var].ID);
                }
            }
        }

        protected void ExportDataProduction(BinaryWriter stream, Rule rule, int length)
        {
            stream.Write((ushort)variables.IndexOf(rule.Head));
            if (rule.ReplaceOnProduction) stream.Write((byte)LRProduction.HeadReplace);
            else stream.Write((byte)LRProduction.HeadKeep);
            stream.Write((byte)length);
            byte bcl = 0;
            int pop = 0;
            foreach (RuleBodyElement elem in rule.Body.Parts)
            {
                if (elem.Symbol is Virtual || elem.Symbol is Action)
                    bcl += 4;
                else if (pop >= length)
                    bcl += 4;
                else
                {
                    bcl += 2;
                    pop++;
                }
            }
            stream.Write(bcl);
            pop = 0;
            foreach (RuleBodyElement elem in rule.Body.Parts)
            {
                if (elem.Symbol is Virtual)
                {
                    if (elem.Action == RuleBodyElementAction.Drop) stream.Write(LRProduction.VirtualDrop);
                    else if (elem.Action == RuleBodyElementAction.Promote) stream.Write(LRProduction.VirtualPromote);
                    else stream.Write(LRProduction.VirtualNoAction);
                    stream.Write((ushort)virtuals.IndexOf(elem.Symbol as Virtual));
                }
                else if (elem.Symbol is Action)
                {
                    stream.Write(LRProduction.SemanticAction);
                    stream.Write((ushort)actions.IndexOf(elem.Symbol as Action));
                }
                else if (pop >= length)
                {
                    // Here the symbol must be a variable
                    ushort index = (ushort)variables.IndexOf(elem.Symbol as CFVariable);
                    if (elem.Action == RuleBodyElementAction.Drop) stream.Write(LRProduction.NullVariableDrop);
                    else if (elem.Action == RuleBodyElementAction.Promote) stream.Write(LRProduction.NullVariablePromote);
                    else stream.Write(LRProduction.NullVariableNoAction);
                    stream.Write(index);
                }
                else
                {
                    if (elem.Action == RuleBodyElementAction.Drop) stream.Write(LRProduction.PopDrop);
                    else if (elem.Action == RuleBodyElementAction.Promote) stream.Write(LRProduction.PopPromote);
                    else stream.Write(LRProduction.PopNoAction);
                    pop++;
                }
            }
        }
    }
}
