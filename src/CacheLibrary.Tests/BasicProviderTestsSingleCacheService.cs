using System;
using System.Collections.Generic;
using System.Linq;
using CacheLibrary.CacheHelper;
using CacheLibrary.CacheProvider;
using CacheLibrary.CacheService;
using NUnit.Framework;
using StackExchange.Redis;

namespace CacheLibrary.Tests
{
    [TestFixture]
    public class BasicProviderTestsSingleCacheService
    {
        private ICacheProvider cacheProvider;

        [SetUp]
        public void Setup()
        {
            cacheProvider = new BasicCacheProvider(new Dictionary<int, ICacheService>()
            {
                {1, new StackExchangeRedisService("localhost:6379") }
            }, new ProtobufBinarySerializer());
        }


        [Test]
        public void redis_service_connection_available()
        {
            var cacheService = cacheProvider.GetServices().First() as CacheServiceBase<ConnectionMultiplexer>;
            cacheService.CacheEndpoint.GetDatabase().Ping();
            Assert.True(true);
        }
        [Test]
        public void get_only()
        {
            var result = cacheProvider.Get<string>(new CacheKey(GetType().FullName + "::PrimitiveReference"));
            Assert.True(true);
        }

        [Test]
        public void execute_cached_for_delete_only()
        {
            cacheProvider.ExecuteCached(new CacheKey(GetType().FullName + "::PrimitiveReference"), () => "", TimeSpan.FromMilliseconds(1));
            Assert.True(true);
        }

        [Test]
        public void execute_cached_without_timeout()
        {
            cacheProvider.ExecuteCached(new CacheKey(GetType().FullName + "::PrimitiveReference"), () => "");
            Assert.True(true);
        }

        [Test]
        public void execute_cached_with_ten_seconds_timeout()
        {
            cacheProvider.ExecuteCached(new CacheKey(GetType().FullName + "::PrimitiveReference"), () => "", TimeSpan.FromSeconds(10));
            Assert.True(true);
        }

        [Test]
        public void execute_cached_with_bypass_cache_parameter()
        {
            var result = cacheProvider.ExecuteCached(new CacheKey(GetType().FullName + "::PrimitiveReference"), () => Guid.NewGuid(), TimeSpan.FromSeconds(10));
            var result2 = cacheProvider.ExecuteCached(new CacheKey(GetType().FullName + "::PrimitiveReference"), () => Guid.NewGuid(), TimeSpan.FromSeconds(10), true);
            var result2Copy = cacheProvider.Get<Guid>(new CacheKey(GetType().FullName + "::PrimitiveReference"));
            Assert.AreEqual(result2Copy.ToString(), result2.ToString());
            Assert.AreNotEqual(result2.ToString(), result.ToString());
        }
    }
}