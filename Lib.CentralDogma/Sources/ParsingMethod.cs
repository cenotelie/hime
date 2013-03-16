/*
 * Author: Charles Hymans
 * Date: 06/08/2011
 * Time: 22:30
 * 
 */

namespace Hime.CentralDogma
{
    /// <summary>
    /// Represents a parsing method
    /// </summary>
	public enum ParsingMethod : byte
    {
        /// <summary>
        /// The LR(0) parsing method
        /// </summary>
        LR0 = 1,
        /// <summary>
        /// The LR(1) parsing method
        /// </summary>
        LR1 = 2,
        /// <summary>
        /// The LALR(1) parsing method
        /// </summary>
        LALR1 = 3,
        /// <summary>
        /// The RNGLR parsing method based on a LR(1) graph
        /// </summary>
        RNGLR1 = 4,
        /// <summary>
        /// The RNGLR parsing method based on a LALR(1) graph
        /// </summary>
        RNGLALR1 = 0,
        /// <summary>
        /// The LR(*) parsing method (experimental)
        /// </summary>
        LRStar = 5
    }
}
