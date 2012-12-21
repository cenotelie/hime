/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
namespace Hime.Redist.AST
{
    /// <summary>
    /// Specifies the tree action for a given node
    /// </summary>
    public enum CSTAction : ushort
    {
        /// <summary>
        /// Promote the node to the immediately upper level in the tree
        /// </summary>
        Promote = Parsers.LRBytecode.PopPromote,
        /// <summary>
        /// Drop the node and all the children from the tree
        /// </summary>
        Drop = Parsers.LRBytecode.PopDrop,
        /// <summary>
        /// Replace the node by its children
        /// </summary>
        Replace = Parsers.LRProduction.HeadReplace,
        /// <summary>
        /// Default action for a node, do nothing
        /// </summary>
        Nothing = Parsers.LRBytecode.PopNoAction
    }
}