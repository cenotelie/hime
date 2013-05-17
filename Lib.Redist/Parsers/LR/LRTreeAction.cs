namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a tree action
    /// </summary>
    public enum LRTreeAction : byte
    {
        /// <summary>
        /// Keep the node as is
        /// </summary>
        None = 0,
        /// <summary>
        /// Replace the node with its children
        /// </summary>
        Replace = 1,
        /// <summary>
        /// Drop the node and all its descendants
        /// </summary>
        Drop = 2,
        /// <summary>
        /// Promote the node, i.e. replace its parent with it and insert its children where it was
        /// </summary>
        Promote = 3
    }
}
