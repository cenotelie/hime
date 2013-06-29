using System;

namespace Hime.Redist.Parsers
{
    class GSS
    {
    	private const int initPathsCount = 64;
        private const int initNodePoolSize = 256;

        private int nbstates;
        private int nextIndex;
    	private Utils.ArrayPool<SPPF> pool128;
        private Utils.ArrayPool<SPPF> pool1024;
        private GSSNode[] free;
        private int nextFree;
        private int allocated;
    	private GSSPath[] paths;
    	
    	public GSS(int nbstates)
    	{
            this.nbstates = nbstates;
            this.nextIndex = 0;
            this.pool128 = new Utils.ArrayPool<SPPF>(128, 128);
            this.pool1024 = new Utils.ArrayPool<SPPF>(1024, 128);
            this.free = new GSSNode[initNodePoolSize];
            this.nextFree = -1;
            this.allocated = 0;
    		this.paths = new GSSPath[initPathsCount];
    	}

        public GSSGeneration GetNextGen() { return new GSSGeneration(this, nextIndex++, nbstates); }

        public GSSNode Acquire()
        {
            if (nextFree == -1)
            {
                // Create new one
                allocated++;
                return new GSSNode();
            }
            else
            {
                return free[nextFree--];
            }
        }

        public void Free(GSSNode node)
        {
            nextFree++;
            if (nextFree == free.Length)
            {
                GSSNode[] temp = new GSSNode[allocated];
                Array.Copy(free, temp, free.Length);
                free = temp;
            }
            free[nextFree] = node;
        }

    	private GSSPath AcquirePath(int length)
    	{
            if (length <= 128)
                return new GSSPath(pool128);
            else if (length <= 1024)
                return new GSSPath(pool1024);
    		return new GSSPath(new SPPF[length]);
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
    			paths[0] = new GSSPath(from);
    			return new GSSPaths(1, paths);
    		}
    		
    		paths[0] = AcquirePath(length);
    		paths[0].Last = from;
    		
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
                        paths[next] = AcquirePath(length);
                        paths[next].Last = edge.To;
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