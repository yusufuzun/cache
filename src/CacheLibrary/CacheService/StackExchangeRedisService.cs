/*
 * StackExchange.Redis library service implementation
 * For license information please check here: 
 * https://github.com/StackExchange/StackExchange.Redis/blob/master/LICENSE
 * 
 * For using this service please install StackExchange.Redis package via this command: Install-Package StackExchange.Redis
 * 
 */

using System;
using CacheLibrary.CacheHelper;
using StackExchange.Redis;

namespace CacheLibrary.CacheService
{
    /// <summary>
    /// StackExchange.Redis implementation of CacheServiceBase
    /// </summary>
    public class StackExchangeRedisService : CacheServiceBase<ConnectionMultiplexer>, ICacheService
    {
        public StackExchangeRedisService(string configuration)
        {
            CacheEndpoint = ConnectionMultiplexer.Connect(configuration);
        }

        public CacheValue Get(CacheKey cacheKey)
        {
            var db = CacheEndpoint.GetDatabase();
            return new CacheValue(db.StringGet(cacheKey.ToString()));
        }

        public void Set(CacheKey cacheKey, CacheValue cacheObj)
        {
            Set(cacheKey, cacheObj, new CacheRule());
        }

        public void Set(CacheKey cacheKey, CacheValue cacheObj, TimeSpan expiresIn)
        {
            Set(cacheKey, cacheObj, new CacheRule(expiresIn));
        }

        public void Set(CacheKey cacheKey, CacheValue cacheObj, CacheRule cacheRule)
        {
            var db = CacheEndpoint.GetDatabase();
            db.StringSet(cacheKey.ToString(), cacheObj.Value, cacheRule.ExpiresIn);
        }

        public void Delete(CacheKey cacheKey)
        {
            var db = CacheEndpoint.GetDatabase();
            db.KeyDelete(cacheKey.ToString());
        }

        private void Delete(IDatabase db, CacheKey cacheKey)
        {
            db.KeyDelete(cacheKey.ToString());
        }

        /// <summary>
        /// Clears cache server with given clear criteria
        /// </summary>
        /// <param name="criteria">A query to filter cache keys, works with redis search characters</param>
        public void ClearCache(CacheClearQuery criteria)
        {
            var db = CacheEndpoint.GetDatabase();
            foreach (var endPoint in CacheEndpoint.GetEndPoints())
            {
                var server = CacheEndpoint.GetServer(endPoint);

                foreach (var redisKey in server.Keys(pattern: criteria.Pattern))
                {
                    Delete(db, new CacheKey(redisKey));
                }
            }
        }
    }
}
