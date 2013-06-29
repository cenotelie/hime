namespace Hime.Redist.Parsers
{
    struct GSSEdge
    {
        private GSSNode to;
        private SPPF label;
        
        public GSSNode To { get { return to; } }
        public SPPF Label { get { return label; } }
        
        public GSSEdge(GSSNode node, SPPF label)
        {
        	this.to = node;
        	this.label = label;
        }
    }
}
