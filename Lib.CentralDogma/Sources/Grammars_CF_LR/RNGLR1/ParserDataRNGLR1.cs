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

using System;
using System.IO;
using System.Collections.Generic;
using Hime.Redist;
using Hime.Redist.Parsers;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class ParserDataRNGLR1 : ParserDataLR
    {
        public ParserDataRNGLR1(Reporting.Reporter reporter, CFGrammar gram, Graph graph) : base(reporter, gram, graph) { }

		protected override string BaseClassName { get { return "RNGLRParser"; } }

        protected override void ExportAutomaton(StreamWriter stream, string name, string resource)
        {
            stream.WriteLine("        private static readonly RNGLRAutomaton automaton = RNGLRAutomaton.Find(typeof(" + name + "Parser), \"" + resource + "\");");
        }

        public override void ExportData(BinaryWriter stream)
        {
            List<KeyValuePair<Rule, int>> rules = new List<KeyValuePair<Rule, int>>();
            List<ushort> nullables = new List<ushort>();
            foreach (CFVariable var in grammar.Variables)
            {
                ushort nullIndex = 0xFFFF;
                foreach (CFRule rule in var.CFRules)
                {
                    rules.Add(new KeyValuePair<Rule, int>(rule, rule.CFBody.GetChoiceAt(0).Length));
                    if (rule.CFBody.GetChoiceAt(0).Firsts.Contains(Epsilon.Instance))
                        nullIndex = (ushort)(rules.Count - 1);
                    for (int i = 1; i < rule.CFBody.GetChoiceAt(0).Length; i++)
                        if (rule.CFBody.GetChoiceAt(i).Firsts.Contains(Epsilon.Instance))
                            rules.Add(new KeyValuePair<Rule, int>(rule, i));
                }
                nullables.Add(nullIndex);
            }

            uint total = 0;
            List<uint> offsets = new List<uint>();
            List<ushort> counts = new List<ushort>();
            foreach (State state in graph.States)
                total = ExportDataCountActions(offsets, counts, total, state);

            stream.Write((ushort)variables.IndexOf(grammar.GetVariable(grammar.GetOption("Axiom"))));
            stream.Write((ushort)(terminals.Count + variables.Count));  // Nb of columns
            stream.Write((ushort)graph.States.Count);                   // Nb or rows
            stream.Write((uint)total);                                  // Nb of actions
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

        private uint ExportDataCountActions(List<uint> offsets, List<ushort> counts, uint total, State state)
        {
            Dictionary<Terminal, int> reductionCounters = new Dictionary<Terminal, int>();
            foreach (StateActionReduce reduce in state.Reductions)
            {
                if (reductionCounters.ContainsKey(reduce.Lookahead))
                    reductionCounters[reduce.Lookahead] = reductionCounters[reduce.Lookahead] + 1;
                else
                    reductionCounters.Add(reduce.Lookahead, 1);
            }
            foreach (Terminal t in terminals)
            {
                int count = state.Children.ContainsKey(t) ? 1 : 0;
                if (reductionCounters.ContainsKey(t))
                    count += reductionCounters[t];
                offsets.Add(total);
                counts.Add((ushort)count);
                total += (uint)count;
            }
            foreach (Variable var in variables)
            {
                int count = state.Children.ContainsKey(var) ? 1 : 0;
                offsets.Add(total);
                counts.Add((ushort)count);
                total += (uint)count;
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
                stream.Write((ushort)LRActionCode.Accept);
                stream.Write((ushort)LRActionCode.None);
            }
            for (int i = 1; i != terminals.Count; i++)
            {
                Terminal t = terminals[i];
                if (state.Children.ContainsKey(t))
                {
                    stream.Write((ushort)LRActionCode.Shift);
                    stream.Write((ushort)state.Children[t].ID);
                }
                if (reductions.ContainsKey(t))
                {
                    foreach (StateActionRNReduce reduce in reductions[t])
                    {
                        stream.Write((ushort)LRActionCode.Reduce);
                        stream.Write((ushort)rules.IndexOf(new KeyValuePair<Rule, int>(reduce.ToReduceRule, reduce.ReduceLength)));
                    }
                }
            }
            foreach (Variable var in variables)
            {
                if (state.Children.ContainsKey(var))
                {
                    stream.Write((ushort)LRActionCode.Shift);
                    stream.Write((ushort)state.Children[var].ID);
                }
            }
        }

        protected void ExportDataProduction(BinaryWriter stream, Rule rule, int length)
        {
            stream.Write((ushort)variables.IndexOf(rule.Head));
            if (rule.ReplaceOnProduction) stream.Write((byte)TreeAction.Replace);
            else stream.Write((byte)TreeAction.None);
            stream.Write((byte)length);
            byte bcl = 0;
            int pop = 0;
            foreach (RuleBodyElement elem in rule.Body.Parts)
            {
                if (elem.Symbol is Virtual || elem.Symbol is Action)
                    bcl += 2;
                else if (pop >= length)
                    bcl += 2;
                else
                {
                    bcl += 1;
                    pop++;
                }
            }
            stream.Write(bcl);
            pop = 0;
            foreach (RuleBodyElement elem in rule.Body.Parts)
            {
                if (elem.Symbol is Virtual)
                {
                    if (elem.Action == RuleBodyElementAction.Drop) stream.Write((ushort)LROpCodeValues.AddVirtualDrop);
                    else if (elem.Action == RuleBodyElementAction.Promote) stream.Write((ushort)LROpCodeValues.AddVirtualPromote);
                    else stream.Write((ushort)LROpCodeValues.AddVirtualNoAction);
                    stream.Write((ushort)virtuals.IndexOf(elem.Symbol as Virtual));
                }
                else if (elem.Symbol is Action)
                {
                    stream.Write((ushort)LROpCodeValues.SemanticAction);
                    stream.Write((ushort)actions.IndexOf(elem.Symbol as Action));
                }
                else if (pop >= length)
                {
                    // Here the symbol must be a variable
                    ushort index = (ushort)variables.IndexOf(elem.Symbol as CFVariable);
                    if (elem.Action == RuleBodyElementAction.Drop) stream.Write((ushort)LROpCodeValues.AddNullVariableDrop);
                    else if (elem.Action == RuleBodyElementAction.Promote) stream.Write((ushort)LROpCodeValues.AddNullVariablePromote);
                    else stream.Write((ushort)LROpCodeValues.AddNullVariableNoAction);
                    stream.Write(index);
                }
                else
                {
                    if (elem.Action == RuleBodyElementAction.Drop) stream.Write((ushort)LROpCodeValues.PopStackDrop);
                    else if (elem.Action == RuleBodyElementAction.Promote) stream.Write((ushort)LROpCodeValues.PopStackPromote);
                    else stream.Write((ushort)LROpCodeValues.PopStackNoAction);
                    pop++;
                }
            }
        }
    }
}
