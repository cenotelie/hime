/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Specifies the tree action for a given node
    /// </summary>
    public enum CSTAction : byte
    {
        /// <summary>
        /// Promote the node to the immediately upper level in the tree
        /// </summary>
        Promote = 3,
        /// <summary>
        /// Drop the node and all the children from the tree
        /// </summary>
        Drop = 2,
        /// <summary>
        /// Replace the node by its children
        /// </summary>
        Replace = 1,
        /// <summary>
        /// Default action for a node, do nothing
        /// </summary>
        Nothing = 0
    }
}