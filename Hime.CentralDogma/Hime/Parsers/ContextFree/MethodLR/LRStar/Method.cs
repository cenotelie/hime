using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class MethodLRStar : CFParserGenerator
    {
        private List<System.Threading.Thread> workers;
        private Hime.Kernel.Reporting.Reporter reporter;
        private GLRSimulator simulator;
        private Dictionary<State, DeciderLRStar> deciders;
        private List<State>.Enumerator enumerator;
        private object _lock;


        public string Name { get { return "LR(*)"; } }

        public MethodLRStar() { }

        public ParserData Build(Grammar grammar, Hime.Kernel.Reporting.Reporter reporter) { return Build((CFGrammar)grammar, reporter); }
        public ParserData Build(CFGrammar grammar, Hime.Kernel.Reporting.Reporter reporter)
        {
            reporter.Info("LR(*)", "LR(*) data ...");
            Graph graph = MethodLALR1.ConstructGraph(grammar, reporter);
            reporter.Info("LR(*)", graph.Sets.Count.ToString() + " LALR(1) states constructed.");

            workers = new List<System.Threading.Thread>();
            this.reporter = reporter;
            simulator = new GLRSimulator(graph);
            deciders = new Dictionary<State, DeciderLRStar>();
            enumerator = graph.Sets.GetEnumerator();
            _lock = new object();
            BuildDeciders(System.Environment.ProcessorCount);

            reporter.Info("LR(*)", "Done !");
            return new ParserDataLRStar(this, grammar, graph, deciders);
        }


        private Dictionary<State, DeciderLRStar> BuildDeciders(int threadCount)
        {
            reporter.Info("LR(*)", "Spawning " + threadCount + " thread(s) to build deciders");
            while (threadCount != 0)
            {
                CreateWorker(threadCount);
                threadCount--;
            }
            foreach (System.Threading.Thread thread in workers)
                thread.Join();
            return deciders;
        }

        private State GetNextState()
        {
            lock (_lock)
            {
                if (!enumerator.MoveNext())
                    return null;
                return enumerator.Current;
            }
        }
        private void AddDecider(State state, DeciderLRStar decider)
        {
            lock (deciders)
            {
                deciders.Add(state, decider);
            }
        }
        private void Report(Hime.Kernel.Reporting.Entry entry)
        {
            lock (reporter)
            {
                reporter.Report(entry);
            }
        }

        private void CreateWorker(int id)
        {
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(worker));
            thread.Name = "LR(*) Decider Builder Thread " + id.ToString();
            workers.Add(thread);
            thread.Start();
        }

        private void worker(object param)
        {
            State state = GetNextState();
            while (state != null)
            {
                DeciderLRStar decider = new DeciderLRStar(state);
                decider.Build(simulator);
                AddDecider(state, decider);

                foreach (Conflict conflict in state.Conflicts)
                {
                    if (!decider.IsResolved(conflict))
                        Report(conflict);
                }
                state = GetNextState();
            }
        }
    }
}
