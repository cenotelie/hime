namespace Hime.Parsers
{
    public interface ParserGenerator
    {
        string Name { get; }
        ParserData Build(Grammar grammar, Hime.Kernel.Reporting.Reporter reporter);
    }
}