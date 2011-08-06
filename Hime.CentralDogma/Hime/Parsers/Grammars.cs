using System.Collections.Generic;

namespace Hime.Parsers
{
    public abstract class Grammar : Hime.Kernel.Symbol
    {
        protected Hime.Kernel.QualifiedName completeName;

        public override Hime.Kernel.QualifiedName CompleteName { get { return completeName; } }

        protected override void SymbolSetParent(Hime.Kernel.Symbol symbol){ this.Parent = symbol; }
        protected override void SymbolSetCompleteName(Hime.Kernel.QualifiedName Name) { completeName = Name; }

        public abstract bool Build(CompilationTask options);
    }

    public interface ParserGenerator
    {
        string Name { get; }
        IParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter);
    }
}
