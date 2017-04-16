namespace CacheLibrary.CacheHelper
{
    /// <summary>
    /// Query for cache clearing operations
    /// </summary>
    public class CacheClearQuery
    {
        /// <summary>
        /// Generates a clear query
        /// </summary>
        /// <param name="pattern">Pattern of keys to be cleared from server</param>
        public CacheClearQuery(string pattern)
        {
            Pattern = pattern;
        }
        /// <summary>
        /// Pattern of keys to be cleared from server
        /// </summary>
        public string Pattern { get; private set; }
    }
}