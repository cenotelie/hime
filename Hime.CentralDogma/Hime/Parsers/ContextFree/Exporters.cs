namespace Hime.Parsers.CF
{
    public interface CFParserExporter : ParserExporter
    {
        void Export(string Name, CFGrammar Grammar, CFParseMethod Method, System.IO.StreamWriter Stream);
    }
}