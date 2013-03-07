/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.AST
{
    /// <summary>
    /// Represents a family of children for a SPPF node
    /// </summary>
    internal class SPPFFamily : List<SPPFNode>
    {
        /// <summary>
        /// Parent SPPF node for this family
        /// </summary>
        private SPPFNode parent;

        /// <summary>
        /// Initializes a new instance of the SPPFFamily class with the given parent
        /// </summary>
        /// <param name="parent">Parent of this family</param>
        public SPPFFamily(SPPFNode parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Appends a new child to this family
        /// </summary>
        /// <param name="child">The new child</param>
        public void Append(SPPFNode child)
        {
            child.parent = this.parent;
            child.originalParent = parent.value;
            this.Add(child);
        }

        /// <summary>
        /// Determines whether this family is equivalent to the tested one
        /// </summary>
        /// <param name="family">The tested family</param>
        /// <returns>True if this family is equivalent to the tested one, false otherwise</returns>
        public bool EquivalentTo(SPPFFamily family)
        {
            if (this.Count != family.Count)
                return false;
            for (int i = 0; i != this.Count; i++)
                if (!this[i].EquivalentTo(family[i]))
                    return false;
            return true;
        }
    }
}