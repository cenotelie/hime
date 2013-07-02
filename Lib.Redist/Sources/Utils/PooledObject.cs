namespace Hime.Redist.Utils
{
    /// <summary>
    /// Represents an object in a pool
    /// </summary>
    interface PooledObject
    {
        /// <summary>
        /// Frees this object and returns it to its pool
        /// </summary>
        void Free();
    }
}