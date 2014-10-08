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
package org.xowl.hime.redist.parsers;

import org.xowl.hime.redist.lexer.Automaton;
import org.xowl.hime.redist.lexer.IContextProvider;
import org.xowl.hime.redist.utils.BigList;
import org.xowl.hime.redist.utils.IntBigList;

import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStreamWriter;
import java.io.Writer;
import java.util.*;

/**
 * Represents Graph-Structured Stacks for GLR parsers
 */
class GSS implements IContextProvider {
    /**
     * The initial size of the paths buffer in this GSS
     */
    private static final int INIT_PATHS_COUNT = 64;

    /**
     * Represents an edge in a Graph-Structured Stack
     */
    private static class Edge {
        /**
         * The index of the node from which this edge starts
         */
        public int from;
        /**
         * The index of the node to which this edge arrives to
         */
        public int to;
        /**
         * The label on this edge
         */
        public GSSLabel label;

        /**
         * Initializes this edge
         *
         * @param from Index of the node from which this edge starts
         * @param to   Index of the node to which this edge arrives to
         */
        public Edge(int from, int to, GSSLabel label) {
            this.from = from;
            this.to = to;
            this.label = label;
        }
    }

    /**
     * Represents a generation in a Graph-Structured Stack
     * <p/>
     * Because GSS nodes are always created in the last generation,
     * a generation is basically a span in the list of GSS nodes,
     * i.e. the starting index in the list of nodes and the number of nodes
     */
    public static class Gen {
        /**
         * The start index of this generation in the list of nodes
         */
        public int start;
        /**
         * The number of nodes in this generation
         */
        public int count;

        /**
         * Initializes this generation
         *
         * @param start The start index of this generation in the list of nodes
         */
        public Gen(int start) {
            this.start = start;
            this.count = 0;
        }
    }

    /**
     * Represents a set of paths in this GSS
     */
    public static class PathSet {
        /**
         * The paths in this set
         */
        public GSSPath[] paths;
        /**
         * The number of paths in this set
         */
        public int count;
    }

    /**
     * The nodes in this GSS.
     * A node simply contains the GLR state it represents
     */
    private IntBigList nodes;
    /**
     * The generations in this GSS
     */
    private BigList<Gen> genNodes;
    /**
     * The edges in this GSS
     */
    private BigList<Edge> edges;
    /**
     * The generations for the edges
     */
    private BigList<Gen> genEdges;
    /**
     * Index of the current generation
     */
    private int generation;
    /**
     * A buffer of GSS paths
     */
    private PathSet set;

    /**
     * Initializes the GSS
     */
    public GSS() {
        this.nodes = new IntBigList();
        this.genNodes = new BigList<Gen>(Gen.class, Gen[].class);
        this.edges = new BigList<Edge>(Edge.class, Edge[].class);
        this.genEdges = new BigList<Gen>(Gen.class, Gen[].class);
        this.generation = -1;
        this.set = new PathSet();
        this.set.paths = new GSSPath[INIT_PATHS_COUNT];
        this.set.paths[0] = new GSSPath(1);
    }

    /**
     * Gets whether the specified context is in effect
     *
     * @param context A context
     * @return <code>true</code>  if the specified context is in effect
     */
    @Override
    public boolean isWithin(int context) {
        return (context == Automaton.DEFAULT_CONTEXT);
    }

    /**
     * Gets the data of the given generation
     *
     * @param generation A generation
     * @return The generation's first node
     */
    public Gen getGeneration(int generation) {
        return genNodes.get(generation);
    }

    /**
     * Gets the GLR state represented by the given node
     *
     * @param node A node
     * @return The GLR state represented by the node
     */
    public int getRepresentedState(int node) {
        return nodes.get(node);
    }

    /**
     * Finds in the given generation contains a node representing the given GLR state
     *
     * @param generation A generation
     * @param state      A GLR state
     * @return The node representing the GLR state, or -1 if it is not found
     */
    public int findNode(int generation, int state) {
        Gen data = genNodes.get(generation);
        for (int i = data.start; i != data.start + data.count; i++)
            if (nodes.get(i) == state)
                return i;
        return -1;
    }

    /**
     * Determines whether this instance has the required edge
     *
     * @param generation The generation of the edge's start node
     * @param from       The edge's start node
     * @param to         The edge's target node
     * @return true if this instance has the required edge; otherwise, false
     */
    public boolean hasEdge(int generation, int from, int to) {
        Gen data = genNodes.get(generation);
        for (int i = data.start; i != data.start + data.count; i++) {
            Edge edge = edges.get(i);
            if (edge.from == from && edge.to == to)
                return true;
        }
        return false;
    }

    /**
     * Opens a new generation in this GSS
     *
     * @return The index of the new generation
     */
    public int createGeneration() {
        genNodes.add(new Gen(nodes.size()));
        genEdges.add(new Gen(edges.size()));
        generation++;
        return generation;
    }

    /**
     * Creates a new node in the GSS
     *
     * @param state The GLR state represented by the node
     * @return The node identifier
     */
    public int createNode(int state) {
        nodes.add(state);
        Gen data = genNodes.get(generation);
        data.count++;
        return nodes.size() - 1;
    }

    /**
     * Creates a new edge in the GSS
     *
     * @param from  The edge's starting node
     * @param to    The edge's target node
     * @param label The edge's label
     */
    public void createEdge(int from, int to, GSSLabel label) {
        edges.add(new Edge(from, to, label));
        Gen data = genEdges.get(generation);
        data.count++;
    }

    /**
     * Setups a reusable GSS path with the given length
     *
     * @param index  The index in the buffer of reusable paths
     * @param last   The last GLR state in the path
     * @param length The path's length
     */
    private void setupPath(int index, int last, int length) {
        if (index >= set.paths.length)
            set.paths = Arrays.copyOf(set.paths, set.paths.length + INIT_PATHS_COUNT);
        if (set.paths[index] == null)
            set.paths[index] = new GSSPath(length);
        else
            set.paths[index].ensure(length);
        set.paths[index].setLast(last);
        set.paths[index].setGeneration(getGenerationOf(last));
    }

    /**
     * Retrieve the generation of the given node in this GSS
     *
     * @param node A node's index
     * @return The index of the generation containing the node
     */
    private int getGenerationOf(int node) {
        for (int i = generation; i != -1; i--) {
            Gen gen = genNodes.get(i);
            if (node >= gen.start && node < gen.start + gen.count)
                return i;
        }
        // should node happen
        return -1;
    }

    /**
     * Gets all paths in the GSS starting at the given node and with the given length
     *
     * @param from   The starting node
     * @param length The length of the requested paths
     * @return A set of paths in this GSS
     */
    public PathSet getPaths(int from, int length) {
        if (length == 0) {
            // use the common 0-length GSS path to avoid new memory allocation
            set.paths[0].setLast(from);
            set.count = 1;
            return set;
        }

        // Initializes the first path
        setupPath(0, from, length);

        // The number of paths in the list
        int total = 1;
        // For the remaining hops
        for (int i = 0; i != length; i++) {
            int m = 0;          // Insertion index for the compaction process
            int next = total;   // Insertion index for new paths
            for (int p = 0; p != total; p++) {
                int last = set.paths[p].getLast();
                int genIndex = set.paths[p].getGeneration();
                // Look for new additional paths from last
                Gen gen = genEdges.get(genIndex);
                int firstEdgeTarget = -1;
                GSSLabel firstEdgeLabel = null;
                for (int e = gen.start; e != gen.start + gen.count; e++) {
                    Edge edge = edges.get(e);
                    if (edge.from == last) {
                        if (firstEdgeTarget == -1) {
                            // This is the first edge
                            firstEdgeTarget = edge.to;
                            firstEdgeLabel = edge.label;
                        } else {
                            // Not the first edge
                            // Clone and extend the new path
                            setupPath(next, edge.to, length);
                            set.paths[next].copyLabelsFrom(set.paths[p], i);
                            set.paths[next].set(i, edge.label);
                            // Go to next insert
                            next++;
                        }
                    }
                }
                // Check whether there was at least one edge
                if (firstEdgeTarget != -1) {
                    // Continue the current path
                    if (m != p) {
                        GSSPath t = set.paths[m];
                        set.paths[m] = set.paths[p];
                        set.paths[p] = t;
                    }
                    set.paths[m].setLast(firstEdgeTarget);
                    set.paths[m].setGeneration(getGenerationOf(firstEdgeTarget));
                    set.paths[m].set(i, firstEdgeLabel);
                    // goto next
                    m++;
                }
            }
            if (m != total) {
                // if some previous paths have been removed
                // => compact the list if needed
                for (int p = total; p != next; p++) {
                    GSSPath t = set.paths[m];
                    set.paths[m] = set.paths[p];
                    set.paths[p] = t;
                    m++;
                }
                // m is now the exact number of paths
                total = m;
            } else if (next != total) {
                // no path has been removed, but some have been added
                // => next is the exact number of paths
                total = next;
            }
        }

        set.count = total;
        return set;
    }

    /**
     * Prints this stack onto the console output
     */
    public void print() {
        try {
            OutputStreamWriter writer = new OutputStreamWriter(System.out, "UTF-8");
            printTo(writer);
            writer.close();
        } catch (IOException ex) {
            // ignore
        }
    }

    /**
     * Prints this stack into the specified file
     *
     * @param file The file to print to
     * @throws IOException When failing to write
     */
    public void printTo(String file) throws IOException {
        FileOutputStream stream = new FileOutputStream(file, false);
        OutputStreamWriter writer = new OutputStreamWriter(stream, "UTF-8");
        printTo(writer);
        writer.close();
        stream.close();
    }

    /**
     * Prints this stack with the specified writer
     *
     * @param writer A writer
     * @throws IOException When failing to write
     */
    public void printTo(Writer writer) throws IOException {
        // list of all nodes having at least one child
        Set<Integer> linked = new HashSet<Integer>();

        for (int i = generation; i != -1; i--) {
            writer.write("--- generation " + i + " ---" + System.lineSeparator());
            // Retrieve the edges in this generation
            Map<Integer, List<Integer>> myedges = new HashMap<Integer, List<Integer>>();
            Gen cedges = genEdges.get(i);
            for (int j = 0; j != cedges.count; j++) {
                Edge edge = this.edges.get(cedges.start + j);
                if (!myedges.containsKey(edge.from))
                    myedges.put(edge.from, new ArrayList<Integer>());
                myedges.get(edge.from).add(edge.to);
                linked.add(edge.to);
            }
            // Retrieve the nodes in this generation and reverse their order
            Gen cnodes = genNodes.get(i);
            List<Integer> mynodes = new ArrayList<Integer>();
            for (int j = 0; j != cnodes.count; j++)
                mynodes.add(cnodes.start + j);
            Collections.reverse(mynodes);
            // print this generation
            for (int node : mynodes) {
                String mark = linked.contains(node) ? "node" : "head";
                if (myedges.containsKey(node)) {
                    for (int to : myedges.get(node)) {
                        int gen = getGenerationOf(to);
                        if (gen == i)
                            writer.write("\t" + mark + " " + nodes.get(node) + " to " + nodes.get(to) + System.lineSeparator());
                        else
                            writer.write("\t" + mark + " " + nodes.get(node) + " to " + nodes.get(to) + " in gen " + gen + System.lineSeparator());
                    }
                } else {
                    writer.write("\t" + mark + " " + nodes.get(node) + System.lineSeparator());
                }
            }
        }
    }
}
