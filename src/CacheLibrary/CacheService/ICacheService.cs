using System;
using CacheLibrary.CacheHelper;

namespace CacheLibrary.CacheService
{
    /// <summary>
    /// basic functions for a cache service
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Gets the value of cache key
        /// </summary>
        /// <param name="cacheKey">Key of cached object</param>
        /// <returns>Returns cache object</returns>
        CacheValue Get(CacheKey cacheKey);

        /// <summary>
        /// Sets the given cache object with its key
        /// </summary>
        /// <param name="cacheKey">Key to be used</param>
        /// <param name="cacheObj">value to be stored</param>
        void Set(CacheKey cacheKey, CacheValue cacheObj);

        /// <summary>
        /// Sets the given cache object with its key
        /// </summary>
        /// <param name="cacheKey">Key to be used</param>
        /// <param name="cacheObj">value to be stored</param>
        /// <param name="expiresIn">Sliding time of cache to being invalidated by server</param>
        void Set(CacheKey cacheKey, CacheValue cacheObj, TimeSpan expiresIn);

        /// <summary>
        /// Sets the given cache object with its key
        /// </summary>
        /// <param name="cacheKey">Key to be used</param>
        /// <param name="cacheObj">value to be stored</param>
        /// <param name="cacheRule"></param>
        void Set(CacheKey cacheKey, CacheValue cacheObj, CacheRule cacheRule);

        /// <summary>
        /// Deletes the cache from server
        /// </summary>
        /// <param name="cacheKey">Cache key to be deleted</param>
        void Delete(CacheKey cacheKey);

        /// <summary>
        /// Clears cache server with given clear criteria
        /// </summary>
        /// <param name="criteria">A query to filter cache keys</param>
        void ClearCache(CacheClearQuery criteria);

    }
}