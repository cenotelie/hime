/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using Hime.Kernel.Reporting;

namespace Hime.Parsers.ContextFree.LR
{
	// TODO: should remove all subclasses of BaseMethod and transforms this into data
	// for instance a dictionary from method name to an instance...
    abstract class BaseMethod : CFParserGenerator
    {
        private List<System.Threading.Thread> workers;
        private List<State>.Enumerator enumerator;
        private object _lock;

        protected GLRSimulator simulator;
        protected Reporter reporter;
        protected Graph graph;
		
		private string name;

        public string Name { get { return this.name; } }
		
		protected BaseMethod(string name, Reporter reporter)
		{
			this.name = name;
			this.reporter = reporter;
		}
		
		// TODO: should register the reporter during class construction!!
        public ParserData Build(CFGrammar grammar, Reporter reporter)
		{
			this.reporter = reporter;
            this.ReportInfo("Constructing " + this.Name + " data ...");
			this.graph = this.BuildGraph(grammar);
            Close();
            this.ReportInfo(this.graph.States.Count.ToString() + " states explored.");
            this.ReportInfo("Done !");
			return BuildParserData(grammar);
		}
		
		protected abstract Graph BuildGraph(CFGrammar grammar);
		protected abstract ParserData BuildParserData(CFGrammar grammar);
		
		protected void ReportInfo(string message)
		{
			this.reporter.Info(this.Name, message);
		}
		
        public ParserData Build(Grammar grammar, Reporter reporter) { return Build((CFGrammar)grammar, reporter); }

        protected virtual void OnBeginState(State state) { }
        protected virtual void OnConflictTreated(State state, Conflict conflict) { }

        protected void Close()
        {
            simulator = new GLRSimulator(graph);
            workers = new List<System.Threading.Thread>();
            enumerator = graph.States.GetEnumerator();
            _lock = new object();

            int threadCount = System.Environment.ProcessorCount;
            reporter.Info(Name, "Spawning " + threadCount + " threads for data building");
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
		
        private void Report(Entry entry)
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