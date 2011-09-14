using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Demo.Tasks
{
    class Daemon : IExecutable
    {
        private string directory;

        public Daemon(string directory)
        {
            this.directory = directory;
        }

        public void Execute()
        {
            Hime.Kernel.KernelDaemon.GenerateNextStep(directory);
        }
    }
}
