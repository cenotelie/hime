namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents an edge in a GSS
	/// </summary>
    struct GSSEdge
    {
        private GSSNode to;
        private SubSPPF label;
        
        /// <summary>
        /// Gets the target node of this edge
        /// </summary>
        public GSSNode To { get { return to; } }
        /// <summary>
        /// Gets the label attached to this edge
        /// </summary>
        public SubSPPF Label { get { return label; } }
        
        /// <summary>
        /// Initializes this edge
        /// </summary>
        /// <param name="node">The edge's target</param>
        /// <param name="label">The edge's label</param>
        public GSSEdge(GSSNode node, SubSPPF label)
        {
        	this.to = node;
        	this.label = label;
        }
    }
}
