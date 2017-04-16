using System;

namespace CacheLibrary.CacheHelper
{
    /// <summary>
    /// A ruleset for any cached object that keeps policies inside it.
    /// </summary>
    public class CacheRule
    {
        /// <summary>
        /// Empty ruleset
        /// </summary>
        public CacheRule()
        {
        }

        /// <summary>
        /// Creates rule for sliding cache operations
        /// </summary>
        /// <param name="expiresIn">TimeSpan value of cache invalidation</param>
        /// <param name="bypassCache">If true executes caching function even though there is a found value in cache server</param>
        public CacheRule(TimeSpan expiresIn, bool bypassCache)
        {
            ExpiresIn = expiresIn;
            BypassCache = bypassCache;
        }
        
        /// <summary>
        /// Creates rule for sliding cache operations
        /// </summary>
        /// <param name="expiresIn">TimeSpan value of cache invalidation</param>
        public CacheRule(TimeSpan expiresIn)
        {
            ExpiresIn = expiresIn;
        }

        public TimeSpan? ExpiresIn { get; }
        public bool BypassCache { get; private set; }

        public CacheRule(bool delete)
        {
            ToDelete = delete;
        }

        public bool ToDelete { get; private set; }

        public static CacheRule Delete()
        {
            return new CacheRule(true);
        }
    }
}
