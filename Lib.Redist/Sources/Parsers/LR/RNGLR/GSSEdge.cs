namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents an edge in a GSS
	/// </summary>
    struct GSSEdge
    {
        private GSSNode to;
        private SPPF label;
        
        /// <summary>
        /// Gets the target node of this edge
        /// </summary>
        public GSSNode To { get { return to; } }
        /// <summary>
        /// Gets the label attached to this edge
        /// </summary>
        public SPPF Label { get { return label; } }
        
        /// <summary>
        /// Initializes this edge
        /// </summary>
        /// <param name="node">The edge's target</param>
        /// <param name="label">The edge's label</param>
        public GSSEdge(GSSNode node, SPPF label)
        {
        	this.to = node;
        	this.label = label;
        }
    }
}
