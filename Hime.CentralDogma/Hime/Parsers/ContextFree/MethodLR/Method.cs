namespace Hime.Parsers.CF.LR
{
    public interface LRParseMethod : CFParseMethod
    {
        LRGraph Graph { get; }
    }
}