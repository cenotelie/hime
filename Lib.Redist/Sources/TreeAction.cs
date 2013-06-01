namespace Hime.Redist
{
    /// <summary>
    /// Represents a tree action
    /// </summary>
    public enum TreeAction : byte
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
        Promote = 3,
        /// <summary>
        /// Execute a semantic action
        /// </summary>
        Semantic = 4
    }
}
