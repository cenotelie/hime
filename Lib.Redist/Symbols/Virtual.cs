namespace Hime.Redist.Symbols
{
    /// <summary>
    /// Represents a synthetic symbol in a grammar
    /// </summary>
    public sealed class Virtual : Symbol
    {
        /// <summary>
        /// Initializes a new instance of the Virtual class with a name
        /// </summary>
        /// <param name="name">Symbol's name</param>
        public Virtual(string name) : base(0, name) { }
		
        /// <summary>
        /// Serves as a hash function for a particular type
        /// </summary>
        /// <returns>A hash code for the current symbol</returns>
        public override int GetHashCode() 
		{ 
			return this.Name.GetHashCode(); 
		}
		
        /// <summary>
        /// Determines whether the specified symbol is equal to the current symbol
        /// </summary>
        /// <param name="obj">The symbol to compare with the current symbol</param>
        /// <returns>true if the specified symbol is equal to the current symbol; otherwise, false</returns>
        public override bool Equals(object obj)
        {
            Virtual other = obj as Virtual;
            if (other == null) return false;
            return (this.Name == other.Name);
        }
    }
}