using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class DecidersBuilder
    {
        private List<System.Threading.Thread> workers;
        private Hime.Kernel.Reporting.Reporter reporter;
        private GLRSimulator simulator;
        private Dictionary<State, DeciderLRStar> deciders;
        private List<State>.Enumerator enumerator;
        private object _lock;

        public DecidersBuilder(Graph graph, Hime.Kernel.Reporting.Reporter reporter)
        {
            workers = new List<System.Threading.Thread>();
            this.reporter = reporter;
            simulator = new GLRSimulator(graph);
            deciders = new Dictionary<State, DeciderLRStar>();
            enumerator = graph.Sets.GetEnumerator();
            _lock = new object();
        }

        public Dictionary<State, DeciderLRStar> Build(int count)
        {
            while (count != 0)
            {
                CreateWorker(count);
                count--;
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