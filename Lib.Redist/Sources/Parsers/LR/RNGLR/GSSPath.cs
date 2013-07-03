using System;
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a path in a GSS
	/// </summary>
    class GSSPath
    {
    	private Pool<GSSPath> pool;
        private GSSNode last;
        private SPPF[] labels;
        
        /// <summary>
        /// Gets or sets the final target of this path
        /// </summary>
        public GSSNode Last
        {
        	get { return last; }
        	set { last = value; }
        }
        
        /// <summary>
        /// Gets or sets the i-th label of the edges traversed by this path
        /// </summary>
        public SPPF this[int index]
        {
        	get { return labels[index]; }
        	set { labels[index] = value; }
        }
        
		/// <summary>
		/// Initializes this path
		/// </summary>
		/// <param name="pool">The parent pool</param>
		/// <param name="capacity">The maximal length of this path</param>
        public GSSPath(Pool<GSSPath> pool, int capacity)
        {
        	this.pool = pool;
        	this.last = null;
        	this.labels = new SPPF[capacity];
        }

        /// <summary>
		/// Initializes this path
		/// </summary>
		/// <param name="capacity">The maximal length of this path</param>
        public GSSPath(int capacity)
        {
            this.pool = null;
            this.last = null;
            this.labels = new SPPF[capacity];
        }

        /// <summary>
		/// Initializes this path
		/// </summary>
        public GSSPath()
        {
        	this.pool = null;
        	this.last = null;
        	this.labels = null;
        }
        
        /// <summary>
        /// Copy the content of another path to this one
        /// </summary>
        /// <param name="path">The path to copy</param>
        /// <param name="length">The path's length</param>
        public void CopyLabelsFrom(GSSPath path, int length)
        {
        	Array.Copy(path.labels, this.labels, length);
        }
        
        /// <summary>
        /// Frees this path and returns it ot its pool
        /// </summary>
        public void Free()
        {
        	if (pool != null)
        		pool.Returns(this);
        }
    }
}
