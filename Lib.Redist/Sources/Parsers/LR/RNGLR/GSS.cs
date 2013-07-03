using System;
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
    class GSS
    {
    	private class GSSPathFactory : Factory<GSSPath>
	    {
	        private int capacity;
	        public GSSPathFactory(int capacity) { this.capacity = capacity; }
	        public GSSPath CreateNew(Pool<GSSPath> pool) { return new GSSPath(pool, capacity); }
	    }
    	
    	private class GSSNodeFactory : Factory<GSSNode>
    	{
    		public GSSNode CreateNew(Pool<GSSNode> pool) { return new GSSNode(); }
    	}
    	
    	private const int initPathsCount = 64;
        private const int initNodePoolSize = 256;

        private int nbstates;
        private int nextIndex;
        private GSSPath path0;
    	private Pool<GSSPath> pathsPool128;
        private Pool<GSSPath> pathsPool1024;
        private Pool<GSSNode> nodesPool;
    	private GSSPath[] paths;
    	
    	public GSS(int nbstates)
    	{
            this.nbstates = nbstates;
            this.nextIndex = 0;
            this.path0 = new GSSPath();
            this.pathsPool128 = new Pool<GSSPath>(new GSSPathFactory(128), 128);
            this.pathsPool1024 = new Pool<GSSPath>(new GSSPathFactory(1024), 128);
            this.nodesPool = new Pool<GSSNode>(new GSSNodeFactory(), 1024);
    		this.paths = new GSSPath[initPathsCount];
    	}

        public GSSGeneration GetNextGen() { return new GSSGeneration(this, nextIndex++, nbstates); }

        public GSSNode AcquireNode() { return nodesPool.Acquire(); }

        public void ReturnsNode(GSSNode node) { nodesPool.Returns(node); }

    	private GSSPath AcquirePath(GSSNode last, int length)
    	{
    		GSSPath p = null;
            if (length <= 128)
            	p = pathsPool128.Acquire();
            else if (length <= 1024)
            	p = pathsPool1024.Acquire();
            else
            	p = new GSSPath(length);
    		p.Last = last;
    		p.Length = length;
    		return p;
    	}
    	
    	private void GrowBuffer()
    	{
    		GSSPath[] temp = new GSSPath[paths.Length + initPathsCount];
            System.Array.Copy(paths, temp, paths.Length);
            paths = temp;
    	}
    	
    	public GSSPaths GetPaths(GSSNode from, int length)
    	{
    		if (length == 0)
    		{
    			path0.Last = from;
    			paths[0] = path0;
    			return new GSSPaths(1, paths);
    		}
    		
    		paths[0] = AcquirePath(from, length);
    		
            // The number of paths in the list
            int count = 1;
            // For the remaining hops
            for (int i = 0; i != length; i++)
            {
                int m = 0;          // Insertion index for the compaction process
                int next = count;   // Insertion index for new paths
                for (int p = 0; p != count; p++)
                {
                    GSSNode last = paths[p].Last;
                    // The path stops here
                    if (last.EdgesCount == 0)
                    {
                        // Cleanup
                        paths[p].Free();
                        continue;
                    }
                    // Look for new additional paths
                    for (int j = 1; j != last.EdgesCount; j++)
                    {
                    	GSSEdge edge = last.GetEdge(j);
                        // Extend the list of paths if necessary
                        if (next == paths.Length)
                        	GrowBuffer();
                        // Clone and extend the new path
                        paths[next] = AcquirePath(edge.To, length);
                        paths[next].CopyLabelsFrom(paths[p], i);
                        paths[next][i] = edge.Label;
                        // Go to next insert
                        next++;
                    }
                    // Get the edge at index 0
                    GSSEdge edge0 = last.GetEdge(0);
                    // Continue the current path
                    paths[m] = paths[p];
                    paths[m].Last = edge0.To;
                    paths[m][i] = edge0.Label;
                    // goto next
                    m++;
                }
                // If Some previous paths have been removed (m != count)
                //    and some have been added (next != cout)
                // => Compact the list
                if (m != count && next != count)
                    for (int p = count; p != next; p++)
                        paths[m++] = paths[p];
                // m is now the exact number of paths
                count = m;
            }
            
            return new GSSPaths(count, paths);
    	}
    }
}