using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CacheLibrary.CacheHelper;
using CacheLibrary.CacheProvider;
using CacheLibrary.CacheService;
using NUnit.Framework;
using StackExchange.Redis;

namespace CacheLibrary.Tests
{
    [TestFixture]
    public class RedisPerformanceBenchmarkingTests
    {
        private ICacheProvider cacheProvider;
        private IBinarySerializer binarySerializer;
        private int requestCount = 6000;

        [SetUp]
        public void given_single_redis_instance_with_basic_cache_provider_and_stack_exchange_library()
        {
            binarySerializer = new ProtobufBinarySerializer();
            cacheProvider = new BasicCacheProvider(new Dictionary<int, ICacheService>()
            {
                {1, new StackExchangeRedisService("localhost:6379") }
            }, binarySerializer);
            requestCount = 100000;
        }

        [Test]
        public void when_called_many_get_with_stackexchange_redis_library_string_get_then_write_time_in_ms()
        {
            var stackExchangeClient =
                ((CacheServiceBase<ConnectionMultiplexer>)cacheProvider.GetServices().First()).CacheEndpoint;

            var db = stackExchangeClient.GetDatabase();
            db.StringSet(string.Format("Perf::Default::Client::StackExchange::ConnectionMultiplexer::Get::{0}", requestCount), Guid.NewGuid().ToString());

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < requestCount; i++)
            {
                db.StringGet(string.Format("Perf::Default::Client::StackExchange::ConnectionMultiplexer::Get::{0}", requestCount));
            }
            sw.Stop();

            Console.WriteLine("Request: " + requestCount);
            Console.WriteLine("Time MS: " + sw.ElapsedMilliseconds);
        }

        [Test]
        public void when_called_many_get_with_cache_provider_library_get_then_write_time_in_ms()
        {
            cacheProvider.ExecuteCached(new CacheKey(string.Format("Perf::Default::Client::StackExchange::CacheProvider::Get::{0}", requestCount)), () => Guid.NewGuid().ToString());

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < requestCount; i++)
            {
                cacheProvider.Get<string>(new CacheKey(string.Format("Perf::Default::Client::StackExchange::CacheProvider::Get::{0}", requestCount)));
            }
            sw.Stop();

            Console.WriteLine("Request: " + requestCount);
            Console.WriteLine("Time MS: " + sw.ElapsedMilliseconds);
        }

        [Test]
        public void when_called_many_get_with_cache_provider_library_execute_cached_then_write_time_in_ms()
        {
            cacheProvider.ExecuteCached(new CacheKey(string.Format("Perf::Default::Client::StackExchange::CacheProvider::ExecuteCached::{0}", requestCount)), () => Guid.NewGuid().ToString());

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < requestCount; i++)
            {
                cacheProvider.ExecuteCached(new CacheKey(string.Format("Perf::Default::Client::StackExchange::CacheProvider::ExecuteCached::{0}", requestCount)), () => Guid.NewGuid().ToString());
            }
            sw.Stop();

            Console.WriteLine("Request: " + requestCount);
            Console.WriteLine("Time MS: " + sw.ElapsedMilliseconds);
        }


        [Test]
        public void when_called_many_set_with_stackexchange_redis_library_string_set_then_write_time_in_ms()
        {
            var stackExchangeClient =
                ((CacheServiceBase<ConnectionMultiplexer>)cacheProvider.GetServices().First()).CacheEndpoint;
            var db = stackExchangeClient.GetDatabase();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < requestCount; i++)
            {
                db.StringSet(string.Format("Perf::Default::Client::StackExchange::ConnectionMultiplexer::Set::{0}::{1}", requestCount, i), Guid.NewGuid().ToString());
            }
            sw.Stop();

            Console.WriteLine("Request: " + requestCount);
            Console.WriteLine("Time MS: " + sw.ElapsedMilliseconds);
        }


        [Test]
        public void when_called_many_set_with_cache_service_library_set_then_write_time_in_ms()
        {
            var cacheService = cacheProvider.GetServices().First();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < requestCount; i++)
            {
                cacheService.Set(
                    new CacheKey(string.Format("Perf::Default::Client::StackExchange::CacheService::Set::{0}::{1}", requestCount, i)), 
                    new CacheValue(binarySerializer.Serialize(Guid.NewGuid().ToString())));
            }
            sw.Stop();

            Console.WriteLine("Request: " + requestCount);
            Console.WriteLine("Time MS: " + sw.ElapsedMilliseconds);
        }

        [Test]
        public void when_called_many_set_with_cache_provider_library_execute_cached_then_write_time_in_ms()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < requestCount; i++)
            {
                cacheProvider.ExecuteCached(
                    new CacheKey(string.Format("Perf::Default::Client::StackExchange::CacheProvider::ExecuteCached::{0}::{1}", requestCount, i)), 
                    () => Guid.NewGuid().ToString());
            }
            sw.Stop();

            Console.WriteLine("Request: " + requestCount);
            Console.WriteLine("Time MS: " + sw.ElapsedMilliseconds);
        }
    }
}