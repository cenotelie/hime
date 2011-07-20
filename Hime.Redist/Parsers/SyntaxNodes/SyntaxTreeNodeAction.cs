namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Specifies the tree action for a given node
    /// </summary>
    public enum SyntaxTreeNodeAction
    {
        /// <summary>
        /// Promote the node to the immediately upper level in the tree
        /// </summary>
        Promote,
        /// <summary>
        /// Drop the node and all the children from the tree
        /// </summary>
        Drop,
        /// <summary>
        /// Replace the node by its children
        /// </summary>
        Replace,
        /// <summary>
        /// Default action for a node, do nothing
        /// </summary>
        Nothing
    }
}