using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class BaseMethod
    {
        private List<System.Threading.Thread> workers;
        private List<State>.Enumerator enumerator;
        private object _lock;

        protected GLRSimulator simulator;
        protected Hime.Kernel.Reporting.Reporter reporter;
        protected Graph graph;

        protected virtual void OnBeginState(State state) { }
        protected virtual void OnConflictTreated(State state, Conflict conflict) { }

        protected void Close()
        {
            simulator = new GLRSimulator(graph);
            workers = new List<System.Threading.Thread>();
            enumerator = graph.Sets.GetEnumerator();
            _lock = new object();

            int threadCount = System.Environment.ProcessorCount;
            reporter.Info("Compiler", "Spawning " + threadCount + " threads for data building");
            while (threadCount != 0)
            {
                SpawnWorker(threadCount);
                threadCount--;
            }
            foreach (System.Threading.Thread thread in workers)
                thread.Join();
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
        private void Report(Hime.Kernel.Reporting.Entry entry)
        {
            lock (reporter)
            {
                reporter.Report(entry);
            }
        }

        private void SpawnWorker(int id)
        {
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(worker));
            thread.Name = "LR Finalizer " + id.ToString();
            workers.Add(thread);
            thread.Start();
        }

        private void worker(object param)
        {
            State state = GetNextState();
            while (state != null)
            {
                OnBeginState(state);
                List<Conflict> unresolved = new List<Conflict>();
                foreach (Conflict conflict in state.Conflicts)
                    if (!conflict.IsResolved)
                        unresolved.Add(conflict);
                if (unresolved.Count != 0)
                {
                    List<List<Terminal>> inputs = simulator.GetInputsFor(state);
                    foreach (Conflict conflict in unresolved)
                    {
                        foreach (List<Terminal> input in inputs)
                        {
                            ConflictExample example = new ConflictExample(conflict.ConflictSymbol);
                            example.Input.AddRange(input);
                            conflict.Examples.Add(example);
                        }
                        OnConflictTreated(state, conflict);
                        Report(conflict);
                    }
                }
                state = GetNextState();
            }
        }
    }
}