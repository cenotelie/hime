namespace Hime.Parsers.CF.LR
{
    /// <summary>
    /// Represent a right nulled reduction action for a set of items
    /// </summary>
    public class LRItemSetActionRNReduce : LRItemSetActionReduce
    {
        /// <summary>
        /// Length of the reduction
        /// </summary>
        protected int p_ReduceLength;

        /// <summary>
        /// Get the length of the reduction
        /// </summary>
        public int ReduceLength { get { return p_ReduceLength; } }

        /// <summary>
        /// Constructs the action
        /// </summary>
        /// <param name="OnSymbol">The lookahead</param>
        /// <param name="ToReduce">The rule to be reduced on lookahead</param>
        /// <param name="ReduceLength"></param>
        public LRItemSetActionRNReduce(Terminal Lookahead, CFRule ToReduce, int ReduceLength) : base(Lookahead, ToReduce) { p_ReduceLength = ReduceLength; }

        /// <summary>
        /// Get a XML node representing the action
        /// </summary>
        /// <param name="Doc">The parent document</param>
        /// <param name="Rules">A list of the grammar rules</param>
        /// <returns>The XML node</returns>
        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc, System.Collections.Generic.List<CFRule> Rules)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("Reduction");
            Node.Attributes.Append(Doc.CreateAttribute("Symbol"));
            Node.Attributes.Append(Doc.CreateAttribute("Index"));
            Node.Attributes.Append(Doc.CreateAttribute("ReduceLength"));
            Node.Attributes["Symbol"].Value = p_Lookahead.SID.ToString("X");
            Node.Attributes["Index"].Value = Rules.IndexOf(p_ToReduce).ToString("X");
            Node.Attributes["ReduceLength"].Value = p_ReduceLength.ToString("X");
            return Node;
        }
    }
}