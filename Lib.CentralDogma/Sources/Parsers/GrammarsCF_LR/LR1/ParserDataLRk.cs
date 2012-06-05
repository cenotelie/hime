/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System;
using System.IO;
using System.Collections.Generic;
using Hime.Utils.Reporting;

namespace Hime.Parsers.ContextFree.LR
{
    class ParserDataLRk : ParserDataLR
    {
        public ParserDataLRk(Reporter reporter, CFGrammar gram, Graph graph) : base(reporter, gram, graph) { }

		protected override string GetBaseClassName { get { return "LRkParser"; } }

        protected override void ExportDataTable(BinaryWriter stream)
        {
            foreach (State state in graph.States)
            {
                Dictionary<Terminal, Rule> reductions = new Dictionary<Terminal,Rule>();
                foreach (StateActionReduce reduction in state.Reductions)
                    reductions.Add(reduction.Lookahead, reduction.ToReduceRule);
                foreach (Terminal t in terminals)
                {
                    if (state.Children.ContainsKey(t))
                    {
                        stream.Write((ushort)2);
                        stream.Write((ushort)state.Children[t].ID);
                    }
                    else if (reductions.ContainsKey(t))
                    {
                        stream.Write((ushort)1);
                        stream.Write((ushort)rules.IndexOf(reductions[t]));
                    }
                    else if (reductions.ContainsKey(null))
                    {
                        stream.Write((ushort)1);
                        stream.Write((ushort)rules.IndexOf(reductions[null]));
                    }
                    else
                    {
                        stream.Write((uint)0);
                    }
                }
                foreach (Variable var in variables)
                {
                    if (state.Children.ContainsKey(var))
                    {
                        stream.Write((ushort)2);
                        stream.Write((ushort)state.Children[var].ID);
                    }
                    else
                    {
                        stream.Write((uint)0);
                    }
                }
            }
        }
    }
}
