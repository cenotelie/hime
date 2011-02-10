namespace Hime.Kernel
{
    /// <summary>
    /// Daemon static class
    /// </summary>
    public sealed class KernelDaemon
    {
        public static void GenerateNextStep(string Path)
        {
            // Initialise logs
            Hime.Parsers.GrammarBuildOptions Options = new Hime.Parsers.GrammarBuildOptions("Hime.Kernel.Resources.Parser", new Hime.Parsers.CF.LR.MethodLALR1(), Path + "KernelResources.Parser.cs");
            Hime.Kernel.Reporting.Reporter Reporter = Options.Reporter;

            // Test path
            Path += "\\Daemon\\";
            if (System.IO.Directory.Exists(Path))
                System.IO.Directory.Delete(Path, true);
            System.IO.Directory.CreateDirectory(Path);

            Reporter.Info("Daemon", "Hime Systems Daemon");
            Reporter.Info("Daemon", "output at : " + Path);

            // Checkout resources
            Resources.AccessorSession Session = Resources.ResourceAccessor.CreateCheckoutSession();
            Resources.ResourceAccessor.CheckOut(Session, "Daemon.Kernel.gram", Path + "Kernel.gram");
            Resources.ResourceAccessor.CheckOut(Session, "Daemon.Generators.ContextFreeGrammars.gram", Path + "Generators.ContextFreeGrammars.gram");
            Resources.ResourceAccessor.CheckOut(Session, "Daemon.Generators.ContextSensitiveGrammars.gram", Path + "Generators.ContextSensitiveGrammars.gram");

            // Compile
            Resources.ResourceCompiler Compiler = new Hime.Kernel.Resources.ResourceCompiler();
            Compiler.AddInputFile(Path + "Kernel.gram");
            Compiler.AddInputFile(Path + "Generators.ContextFreeGrammars.gram");
            Compiler.AddInputFile(Path + "Generators.ContextSensitiveGrammars.gram");
            Namespace DaemonRoot = new Namespace(null, "global");
            Compiler.Compile(DaemonRoot, Reporter);

            // Close session
            Session.Close();

            // Generate parser for FileCentralDogma
            Hime.Parsers.Grammar FileCentralDogma = (Hime.Parsers.Grammar)DaemonRoot.ResolveName(QualifiedName.ParseName("Hime.Kernel.FileCentralDogma"));
            FileCentralDogma.Build(Options);

            // Export log
            Reporter.ExportHTML(Path + "DaemonLog.html", "Daemon Log");
            System.Diagnostics.Process.Start(Path + "DaemonLog.html");
        }
    }
}