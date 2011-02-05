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
            Kernel.Logs.LogProxy Log = new Hime.Kernel.Logs.LogProxy();
            Kernel.Logs.LogXML LogXML = new Hime.Kernel.Logs.LogXML();
            Log.AddTarget(Kernel.Logs.LogConsole.Instance);
            Log.AddTarget(LogXML);

            // Test path
            Path += "\\Daemon\\";
            if (System.IO.Directory.Exists(Path))
                System.IO.Directory.Delete(Path, true);
            System.IO.Directory.CreateDirectory(Path);

            Log.SectionBegin("Daemon");
            Log.EntryBegin("Info"); Log.EntryAddData("Daemon"); Log.EntryAddData("Hime Systems Daemon"); Log.EntryEnd();
            Log.EntryBegin("Info"); Log.EntryAddData("Daemon"); Log.EntryAddData("output at : " + Path); Log.EntryEnd();
            Log.SectionEnd();

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
            Compiler.Compile(DaemonRoot, Log);

            // Close session
            Session.Close();

            // Generate parser for FileCentralDogma
            Generators.Parsers.Grammar FileCentralDogma = (Generators.Parsers.Grammar)DaemonRoot.ResolveName(QualifiedName.ParseName("Hime.Kernel.FileCentralDogma"));
            FileCentralDogma.GenerateParser("Hime.Kernel.Resources.Parser", Hime.Generators.Parsers.GrammarParseMethod.LALR1, Path + "KernelResources.Parser.cs", Log);

            // Export log
            LogXML.ExportHTML(Path + "DaemonLog.html", "Daemon Log");
            System.Diagnostics.Process.Start(Path + "DaemonLog.html");
        }
    }
}