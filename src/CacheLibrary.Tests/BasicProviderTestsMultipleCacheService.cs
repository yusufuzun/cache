using System;
using System.Collections.Generic;
using CacheLibrary.CacheHelper;
using CacheLibrary.CacheProvider;
using CacheLibrary.CacheService;
using NUnit.Framework;
using StackExchange.Redis;

namespace CacheLibrary.Tests
{
    [TestFixture]
    public class BasicProviderTestsMultipleCacheService
    {
        private ICacheProvider cacheProvider;
        private IBinarySerializer binarySerializer;

        [SetUp]
        public void Setup()
        {
            binarySerializer = new ProtobufBinarySerializer();
            cacheProvider = new BasicCacheProvider(new Dictionary<int, ICacheService>()
            {
                {1, new StackExchangeRedisService("localhost:6379") },
                {2, new StackExchangeRedisService("localhost:6380") }
            }, binarySerializer);
        }


        [Test]
        public void all_redis_services_connection_available()
        {
            foreach (var service in cacheProvider.GetServices())
            {
                var cacheService = service as CacheServiceBase<ConnectionMultiplexer>;
                cacheService.CacheEndpoint.GetDatabase().Ping();
            }
            Assert.True(true);
        }
        [Test]
        public void get_only()
        {
            var result = cacheProvider.Get<string>(new CacheKey(GetType().FullName + "::PrimitiveReference::GetOnly"));
            Assert.True(true);
        }

        [Test]
        public void execute_cached_for_delete_only()
        {
            cacheProvider.ExecuteCached(new CacheKey(GetType().FullName + "::PrimitiveReference::DeleteOnly"), () => Guid.NewGuid(), CacheRule.Delete());
            Assert.True(true);
            var result = cacheProvider.Get<Guid>(new CacheKey(GetType().FullName + "::PrimitiveReference::DeleteOnly"));
            Assert.AreEqual(Guid.Empty.ToString(), result.ToString());
        }

        [Test]
        public void execute_cached_without_timeout()
        {
            cacheProvider.ExecuteCached(new CacheKey(GetType().FullName + "::PrimitiveReference::WoTimeout"), () => "");
            Assert.True(true);
        }

        [Test]
        public void execute_cached_with_ten_seconds_timeout()
        {
            cacheProvider.ExecuteCached(new CacheKey(GetType().FullName + "::PrimitiveReference::Timeout"), () => "", TimeSpan.FromSeconds(10));
            Assert.True(true);
        }

        [Test]
        public void execute_cached_with_bypass_cache_parameter()
        {
            var result = cacheProvider.ExecuteCached(new CacheKey(GetType().FullName + "::PrimitiveReference::ByPass"), () => Guid.NewGuid(), TimeSpan.FromSeconds(10));
            var result2 = cacheProvider.ExecuteCached(new CacheKey(GetType().FullName + "::PrimitiveReference::ByPass"), () => Guid.NewGuid(), TimeSpan.FromSeconds(10), true);
            var result2Copy = cacheProvider.Get<Guid>(new CacheKey(GetType().FullName + "::PrimitiveReference::ByPass"));
            Assert.AreEqual(result2Copy.ToString(), result2.ToString());
            Assert.AreNotEqual(result2.ToString(), result.ToString());
        }

        [Test]
        public void execute_cached_then_get_from_each()
        {
            var result = cacheProvider.ExecuteCached(new CacheKey(GetType().FullName + "::PrimitiveReference::ExecAndGet"), () => Guid.NewGuid());
            var expected = new byte[] { };
            foreach (var service in cacheProvider.GetServices())
            {
                var co = service.Get(new CacheKey(GetType().FullName + "::PrimitiveReference::ExecAndGet"));
                Assert.NotNull(co);
                Assert.NotNull(co.Value);
                Assert.AreEqual(result, binarySerializer.Deserialize<Guid>(co.Value));
            }
        }
    }
}