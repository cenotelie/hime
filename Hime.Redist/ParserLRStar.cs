using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    public abstract class BaseLRStarParser : IParser
    {
        protected delegate void Production(BaseLRStarParser parser, List<SyntaxTreeNode> nodes);
        protected struct Rule
        {
            public Production OnReduction;
            public SymbolVariable Head;
            public ushort Length;
            public Rule(Production prod, SymbolVariable head, ushort length)
            {
                OnReduction = prod;
                Head = head;
                Length = length;
            }
        }
        protected struct DeciderState
        {
            public Dictionary<ushort, ushort> Transitions;
            public ushort Shift;
            public Rule Reduction;
            public DeciderState(ushort[] t_keys, ushort[] t_val, ushort shift, Rule reduction)
            {
                Transitions = new Dictionary<ushort, ushort>();
                for (int i = 0; i != t_keys.Length; i++)
                    Transitions.Add(t_keys[i], t_val[i]);
                Shift = shift;
                Reduction = reduction;
            }
        }
        protected struct State
        {
            public string[] Items;
            public SymbolTerminal[] Expected;
            public DeciderState[] Decider;
            public Dictionary<ushort, ushort> ShiftsOnVariable;
            public State(string[] items, SymbolTerminal[] expected, DeciderState[] decider, ushort[] sv_keys, ushort[] sv_val)
            {
                Items = items;
                Expected = expected;
                Decider = decider;
                ShiftsOnVariable = new Dictionary<ushort, ushort>();
                for (int i = 0; i != sv_keys.Length; i++)
                    ShiftsOnVariable.Add(sv_keys[i], sv_val[i]);
            }
            public ushort[] GetExpectedIDs()
            {
                ushort[] results = new ushort[Expected.Length];
                for (int i = 0; i != Expected.Length; i++)
                    results[i] = Expected[i].SymbolID;
                return results;
            }
            public string[] GetExpectedNames()
            {
                string[] results = new string[Expected.Length];
                for (int i = 0; i != Expected.Length; i++)
                    results[i] = Expected[i].Name;
                return results;
            }
            public ushort GetNextByShiftOnVariable(ushort sid)
            {
                if (!ShiftsOnVariable.ContainsKey(sid))
                    return 0xFFFF;
                return ShiftsOnVariable[sid];
            }
        }


        // Parser automata data
        protected Rule[] rules;
        protected State[] states;
        protected int errorSimulationLength;

        // Parser state data
        protected int maxErrorCount;
        protected List<ParserError> errors;
        protected System.Collections.ObjectModel.ReadOnlyCollection<ParserError> readonlyErrors;
        protected BufferedTokenReader reader;
        protected List<SyntaxTreeNode> nodes;
        protected Stack<ushort> stack;
        protected SymbolToken nextToken;
        protected ushort currentState;

        public ICollection<ParserError> Errors { get { return readonlyErrors; } }

        protected abstract void setup();

        public BaseLRStarParser(ILexer input)
        {
            setup();
            maxErrorCount = 100;
            errors = new List<ParserError>();
            readonlyErrors = new System.Collections.ObjectModel.ReadOnlyCollection<ParserError>(errors);
            reader = new BufferedTokenReader(input);
            nodes = new List<SyntaxTreeNode>();
            stack = new Stack<ushort>();
            currentState = 0x0;
            nextToken = null;
        }

        protected ushort Analyze_RunDecider(ushort first, out Rule reduction)
        {
            int depth = 0;
            reduction = new Rule();
            State state = states[currentState];
            DeciderState ds = state.Decider[0];
            ushort token = first;
            if (!ds.Transitions.ContainsKey(token)) // Unexpected token !
                return 0xFFFF;
            ds = state.Decider[ds.Transitions[token]];
            while (true)
            {
                if (ds.Shift != 0xFFFF)
                {
                    reader.Rewind(depth);
                    return ds.Shift;
                }
                if (ds.Reduction.Head != null)
                {
                    reduction = ds.Reduction;
                    reader.Rewind(depth);
                    return 0xFFFF;
                }
                // go to new state
                token = reader.Read().SymbolID;
                depth++;
                if (!ds.Transitions.ContainsKey(token))
                {
                    // Unexpected token !
                    reader.Rewind(depth);
                    return 0xFFFF;
                }
                ds = state.Decider[ds.Transitions[token]];
            }
        }

        protected bool Analyse_RunForToken(SymbolToken token)
        {
            while (true)
            {
                Rule reduction;
                ushort NextState = Analyze_RunDecider(token.SymbolID, out reduction);
                if (NextState != 0xFFFF)
                {
                    nodes.Add(new SyntaxTreeNode(token));
                    currentState = NextState;
                    stack.Push(currentState);
                    return true;
                }
                if (reduction.Head != null)
                {
                    Production Reduce = reduction.OnReduction;
                    ushort HeadID = reduction.Head.SymbolID;
                    Reduce(this, nodes);
                    for (ushort j = 0; j != reduction.Length; j++)
                        stack.Pop();
                    // Shift to next state on the reduce variable
                    NextState = states[stack.Peek()].GetNextByShiftOnVariable(HeadID);
                    if (NextState == 0xFFFF)
                        return false;
                    currentState = NextState;
                    stack.Push(currentState);
                    continue;
                }
                return false;
            }
        }

        public SyntaxTreeNode Analyse()
        {
            stack.Push(currentState);
            nextToken = reader.Read();

            while (true)
            {
                if (Analyse_RunForToken(nextToken))
                {
                    nextToken = reader.Read();
                    continue;
                }
                else if (nextToken.SymbolID == 0x0001)
                    return nodes[0].ApplyActions();
                else
                    // Unexpected token
                    return null;
            }
        }
    }
}
