namespace CacheLibrary.CacheService
{
    /// <summary>
    /// Cache service base class
    /// </summary>
    /// <typeparam name="TClient">Client object type for caching service</typeparam>
    public abstract class CacheServiceBase<TClient>
    {
        /// <summary>
        /// Cache server access object
        /// </summary>
        public TClient CacheEndpoint { get; set; }
    }
}