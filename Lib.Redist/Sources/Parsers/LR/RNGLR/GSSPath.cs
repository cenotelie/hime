using System;

namespace Hime.Redist.Parsers
{
    struct GSSPath
    {
    	private Utils.ArrayPool<SPPF> pool;
        private GSSNode last;
        private SPPF[] labels;
        
        public GSSNode Last
        {
        	get { return last; }
        	set { last = value; }
        }
        
        public SPPF this[int index]
        {
        	get { return labels[index]; }
        	set { labels[index] = value; }
        }

        public GSSPath(Utils.ArrayPool<SPPF> pool)
        {
        	this.pool = pool;
        	this.last = null;
        	this.labels = pool.Acquire();
        }

        public GSSPath(SPPF[] labels)
        {
            this.pool = null;
            this.last = null;
            this.labels = labels;
        }

        public GSSPath(GSSNode node)
        {
        	this.pool = null;
        	this.last = node;
        	this.labels = null;
        }
        
        public void CopyLabelsFrom(GSSPath path, int length)
        {
        	Array.Copy(path.labels, this.labels, length);
        }
        
        public void Free()
        {
        	if (pool != null)
        		pool.Returns(labels);
        }
    }
}
