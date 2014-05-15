/**********************************************************************
 * Copyright (c) 2014 Laurent Wouters and others
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation, either version 3
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General
 * Public License along with this program.
 * If not, see <http://www.gnu.org/licenses/>.
 *
 * Contributors:
 *     Laurent Wouters - lwouters@xowl.org
 **********************************************************************/
package hime.redist.parsers;

/**
 * Represents a label on a GSS edge
 *
 * The data in this structure can have two interpretations:
 * 1) It can represent a sub-tree with a replaceable root.
 * 2) It can represent a reference to a single node in a SPPF.
 */
class GSSLabel {
    /**
     * A sub-tree with a replaceable root
     */
    private SubTree tree;
    /**
     * The original symbol of the SPPF node
     */
    private int original;
    /**
     * The index of the SPPF node
     */
    private int nodeIndex;

    /**
     * Gets the sub-tree with a repleaceable root
     * @return The sub-tree with a repleaceable root
     */
    public SubTree getTree() { return tree; }

    /**
     * Gets the original symbol of the SPPF node
     * @return The original symbol of the SPPF node
     */
    public int getOriginal() { return original; }

    /**
     * Gets the index of the SPPF node
     * @return The index of the SPPF node
     */
    public int getIndex() { return nodeIndex; }

    /**
     * Wether this label is an epsilon label
     * @return true if this is an epsilon label
     */
    public boolean isEpsilon() { return (tree == null && nodeIndex == -1); }

    /**
     * Whether this label represents a sub-tree with a replaceable root
     * @return true if this label represents a sub-tree with a replaceable root
     */
    public boolean IsReplaceable() { return (tree != null); }

    /**
     * Initializes this label as representing a sub-tree with a replaceable root
     * @param tree The sub-tree with a replaceable root
     */
    public GSSLabel(SubTree tree)
    {
        this.tree = tree;
        this.original = 0;
        this.nodeIndex = -1;
    }

    /**
     * Initializes this label as representing a single SPPF node
     * @param original The original symbol of the SPPF node
     * @param index The index of the SPPF node
     */
    public GSSLabel(int original, int index)
    {
        this.tree = null;
        this.original = original;
        this.nodeIndex = index;
    }
}
