namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a collection of paths in a GSS
	/// </summary>
    struct GSSPaths
    {
    	private int count;
    	private GSSPath[] buffer;
    	
    	/// <summary>
    	/// Gets the number of paths on this collection
    	/// </summary>
    	public int Count { get { return count; } }
    	/// <summary>
    	/// Gets the i-th path in this collection
    	/// </summary>
    	public GSSPath this[int index] { get { return buffer[index]; } }
    	
    	/// <summary>
    	/// Initializes the collection
    	/// </summary>
    	/// <param name="count">The number of paths</param>
    	/// <param name="buffer">The paths' data</param>
    	public GSSPaths(int count, GSSPath[] buffer)
    	{
    		this.count = count;
    		this.buffer = buffer;
    	}
    }
}