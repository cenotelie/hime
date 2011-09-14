using System.Collections.Generic;

namespace Hime.Parsers
{
    public abstract class Grammar : Hime.Kernel.Naming.Symbol
    {
        protected Hime.Kernel.Naming.QualifiedName completeName;

        public override Hime.Kernel.Naming.QualifiedName CompleteName { get { return completeName; } }

        protected override void SymbolSetParent(Hime.Kernel.Naming.Symbol symbol) { this.Parent = symbol; }
        protected override void SymbolSetCompleteName(Hime.Kernel.Naming.QualifiedName Name) { completeName = Name; }

        public abstract bool Build(CompilationTask options);
    }

    public interface ParserGenerator
    {
        string Name { get; }
        ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter);
    }
}
