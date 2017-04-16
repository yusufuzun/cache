using System;
using System.Collections.Generic;
using System.Threading;
using CacheLibrary.CacheHelper;
using CacheLibrary.CacheProvider;
using CacheLibrary.CacheService;
using NUnit.Framework;
using ProtoBuf.Meta;

namespace CacheLibrary.Tests
{
    [TestFixture]
    public class MoreProviderTestsSingleCacheService
    {
        private ICacheProvider cacheProvider;

        [Serializable]
        class ComplexObj
        {
            public ComplexObj()
            {
                A = "AString";
                B = 111;
            }
            public string A { get; set; }
            public int B { get; set; }
        }

        [SetUp]
        public void Setup()
        {
            RuntimeTypeModel.Default.Add(typeof(ComplexObj), true);
            cacheProvider = new BasicCacheProvider(new Dictionary<int, ICacheService>()
            {
                {1, new StackExchangeRedisService("localhost:6379") }
            }, new ProtobufBinarySerializer());
        }

        [Test]
        public void when_execute_cached_then_get_then_check_result()
        {
            var result = cacheProvider.ExecuteCached(new CacheKey(GetType().FullName + "::ComplexReference"), () => new ComplexObj());
            Assert.AreEqual(new ComplexObj().A, result.A);
            Assert.AreEqual(new ComplexObj().B, result.B);
        }

        [Test]
        public void when_execute_cached_for_five_seconds_then_get_result_null()
        {
            cacheProvider.ExecuteCached(new CacheKey(GetType().FullName + "::ComplexReference"), () => new ComplexObj(), TimeSpan.FromMilliseconds(5000), true);
            Thread.Sleep(5000);
            var result = cacheProvider.Get<ComplexObj>(new CacheKey(GetType().FullName + "::ComplexReference"));
            Assert.IsNull(result);
        }
        
        [Test]
        public void when_deleted_then_execute_cached_then_get_then_check_result()
        {
            var result = cacheProvider.ExecuteCached(new CacheKey(GetType().FullName + "::ComplexReference::Check"),
                () => new ComplexObj(), CacheRule.Delete());

            result = cacheProvider.Get<ComplexObj>(new CacheKey(GetType().FullName + "::ComplexReference::Check"));
            Assert.IsNull(result);

            result = cacheProvider.ExecuteCached(new CacheKey(GetType().FullName + "::ComplexReference::Check"), () => new ComplexObj(), TimeSpan.FromSeconds(4));
            Assert.AreEqual(new ComplexObj().A, result.A);
            Assert.AreEqual(new ComplexObj().B, result.B);

            Thread.Sleep(4000);
            result = cacheProvider.Get<ComplexObj>(new CacheKey(GetType().FullName + "::ComplexReference::Check"));
            Assert.IsNull(result);
        }
    }
}