namespace Hime.Redist.Parsers
{
    struct GSSPaths
    {
    	private int count;
    	private GSSPath[] buffer;
    	
    	public int Count { get { return count; } }
    	public GSSPath this[int index] { get { return buffer[index]; } }
    	
    	public GSSPaths(int count, GSSPath[] buffer)
    	{
    		this.count = count;
    		this.buffer = buffer;
    	}
    }
}