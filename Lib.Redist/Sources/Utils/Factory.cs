namespace Hime.Redist.Utils
{
    /// <summary>
    /// Represents a factory of objects for a pool
    /// </summary>
    /// <typeparam name="T">The type of the pooled objects</typeparam>
    interface Factory<T>
    {
        /// <summary>
        /// Creates a new object
        /// </summary>
        /// <param name="pool">The enclosing pool</param>
        /// <returns>The created object</returns>
        T CreateNew(Pool<T> pool);
    }
}
