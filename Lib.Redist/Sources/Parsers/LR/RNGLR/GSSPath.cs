using System;
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
    class GSSPath
    {
    	private Pool<GSSPath> pool;
        private GSSNode last;
        private SPPF[] labels;
        private int length;
        
        public GSSNode Last
        {
        	get { return last; }
        	set { last = value; }
        }
        
        public int Length
        {
        	get { return length; }
        	set { length = value; }
        }
        
        public SPPF this[int index]
        {
        	get { return labels[index]; }
        	set { labels[index] = value; }
        }

        public GSSPath(Pool<GSSPath> pool, int capacity)
        {
        	this.pool = pool;
        	this.last = null;
        	this.labels = new SPPF[capacity];
        	this.length = 0;
        }

        public GSSPath(int capacity)
        {
            this.pool = null;
            this.last = null;
            this.labels = new SPPF[capacity];
        	this.length = 0;
        }

        public GSSPath()
        {
        	this.pool = null;
        	this.last = null;
        	this.labels = null;
        	this.length = 0;
        }
        
        public void CopyLabelsFrom(GSSPath path, int length)
        {
        	Array.Copy(path.labels, this.labels, length);
        }
        
        public void Free()
        {
        	if (pool != null)
        		pool.Returns(this);
        }
    }
}
