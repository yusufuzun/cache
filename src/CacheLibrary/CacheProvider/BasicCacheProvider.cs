using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CacheLibrary.CacheHelper;
using CacheLibrary.CacheService;

namespace CacheLibrary.CacheProvider
{
    /// <summary>
    /// Caching provider for given number of cache services.
    /// </summary>
    public class BasicCacheProvider : ICacheProvider
    {
        private readonly Dictionary<int, ICacheService> services;
        private readonly IBinarySerializer serializer;

        private readonly int cancellationTimeoutMs; //10 sec

        public BasicCacheProvider(Dictionary<int, ICacheService> services,
            IBinarySerializer serializer,
            int cancellationTimeoutMs = 10000)
        {
            this.cancellationTimeoutMs = cancellationTimeoutMs;
            this.services = services;
            this.serializer = serializer;
        }

        public T ExecuteCached<T>(CacheKey cacheKey, Func<T> cacheFunc)
        {
            return ExecuteCached(cacheKey, cacheFunc, new CacheRule());
        }

        public T ExecuteCached<T>(CacheKey cacheKey, Func<T> cacheFunc, TimeSpan expiresIn)
        {
            return ExecuteCached(cacheKey, cacheFunc, new CacheRule(expiresIn));
        }

        public T ExecuteCached<T>(CacheKey cacheKey, Func<T> cacheFunc, TimeSpan expiresIn, bool bypassCache)
        {
            return ExecuteCached(cacheKey, cacheFunc, new CacheRule(expiresIn, bypassCache));
        }

        public T ExecuteCached<T>(CacheKey cacheKey, Func<T> cacheFunc, CacheRule cacheRule)
        {
            return ExecuteCachedInside(cacheKey, cacheFunc, cacheRule);
        }

        public Task<T> ExecuteCachedAsync<T>(CacheKey cacheKey, Func<T> cacheFunc)
        {
            return Task.Factory.StartNew(() => ExecuteCached(cacheKey, cacheFunc));
        }

        public Task<T> ExecuteCachedAsync<T>(CacheKey cacheKey, Func<T> cacheFunc, TimeSpan expiresIn)
        {
            return Task.Factory.StartNew(() => ExecuteCached(cacheKey, cacheFunc, expiresIn));
        }

        public Task<T> ExecuteCachedAsync<T>(CacheKey cacheKey, Func<T> cacheFunc, TimeSpan expiresIn,
            bool bypassCache)
        {
            return Task.Factory.StartNew(() => ExecuteCached(cacheKey, cacheFunc, expiresIn, bypassCache));
        }

        public Task<T> ExecuteCachedAsync<T>(CacheKey cacheKey, Func<T> cacheFunc, CacheRule cacheRule)
        {
            return Task.Factory.StartNew(() => ExecuteCached(cacheKey, cacheFunc, cacheRule));
        }

        private T ExecuteCachedInside<T>(CacheKey cacheKey, Func<T> cachedItem, CacheRule cacheRule)
        {
            var cachingProviderTokenSource = new CancellationTokenSource(cancellationTimeoutMs);

            var cacheValue = Get<T>(cacheKey);
            if (cacheRule.ToDelete)
            {
                var svcCount = services.Count;
                var serviceTasks = new Task[svcCount];
                for (int i = 0; i < svcCount; i++)
                {
                    var i1 = i;
                    var task = Task.Factory.StartNew(() =>
                    {
                        var svcId = services.ElementAt(i1).Key;
                        var svc = services[svcId];
                        svc.Delete(cacheKey);
                    }, cachingProviderTokenSource.Token);
                    serviceTasks[i1] = task;
                }
                Task.WaitAll(serviceTasks, cachingProviderTokenSource.Token);
                return default(T);
            }
            if (EqualityComparer<T>.Default.Equals(cacheValue, default(T)) || cacheRule.BypassCache)
            {
                var result = cachedItem();
                var svcCount = services.Count;
                var serviceTasks = new Task[svcCount];
                for (int i = 0; i < svcCount; i++)
                {
                    var i1 = i;
                    var task = Task.Factory.StartNew(() =>
                    {
                        var svcId = services.ElementAt(i1).Key;
                        var svc = services[svcId];
                        svc.Set(cacheKey, new CacheValue(serializer.Serialize(result)), cacheRule);
                    }, cachingProviderTokenSource.Token);
                    serviceTasks[i1] = task;
                }
                Task.WaitAll(serviceTasks, cachingProviderTokenSource.Token);
                return result;
            }

            return cacheValue;
        }
        
        public Task<T> GetAsync<T>(CacheKey cacheKey)
        {
            return Task.Factory.StartNew(() => Get<T>(cacheKey),
                new CancellationTokenSource(cancellationTimeoutMs).Token);
        }

        public T Get<T>(CacheKey cacheKey)
        {
            for (int i = 0; i < services.Count; i++)
            {
                var svcId = services.ElementAt(i).Key;
                var svc = services[svcId];
                var result = svc.Get(cacheKey);
                if (result.Value == null)
                {
                    continue;
                }
                return serializer.Deserialize<T>(result.Value);
            }
            return default(T);
        }

        public IEnumerable<ICacheService> GetServices()
        {
            return services.Select(x => x.Value).ToList();
        }
    }
}