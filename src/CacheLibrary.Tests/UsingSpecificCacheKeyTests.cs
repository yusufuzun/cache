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
    public class UsingSpecificCacheKeyTests
    {
        private ICacheProvider cacheProvider;

        [Serializable]
        public class ComplexObj
        {
            public int Id { get; }

            public ComplexObj()
            {
                A = "AString";
                B = 111;
            }

            public ComplexObj(int id) : this()
            {
                Id = id;
            }

            public string A { get; set; }
            public int B { get; set; }
        }

        public class ComplexCacheKey : CacheKey
        {
            public int Id { get; set; }
            public ComplexCacheKey(int id, string key) : base(string.Format("CO::{0}::{1}", id, key))
            {
                Id = id;
            }
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
        public void when_complex_cache_key_send_then_get_with_complex_key_again()
        {
            var id = 222;
            var result = cacheProvider.ExecuteCached(new ComplexCacheKey(id, GetType().FullName + "::ComplexReferenceWithComplexKey"), () => new ComplexObj(id));
            Thread.Sleep(500);
            var result2 = cacheProvider.Get<ComplexObj>(new ComplexCacheKey(id, GetType().FullName + "::ComplexReferenceWithComplexKey"));
            Assert.AreEqual(result2.A, result.A);
            Assert.AreEqual(result2.B, result.B);
        }

    }
}