/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.CentralDogma.Reporting
{
    /// <summary>
    /// Represents an entry's level of gravity
    /// </summary>
    public enum ELevel
    {
        /// <summary>
        /// Informational entry
        /// </summary>
        Info,
        /// <summary>
        /// Warning but non-blocking
        /// </summary>
        Warning,
        /// <summary>
        /// Error preventing the correct behavior of the application
        /// </summary>
        Error
    }
}