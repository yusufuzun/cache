# cache
Basic Cache Facade for generic caching purposes with StackExchange.Redis implementation

# What is this
This library includes very simple form of basic caching operations such as get, set, delete.

By using this library you can add return value of a function to any implemented caching service.

For the sake of simplicity I only implemented StackExchange.Redis library as a Cache Service. But any other caching infrastructure can be used by just implementing ICacheService interface.

# How to use
I simply created some test cases for usage. You can find usage in test files. I explain simple usage below also.

## Initial Registrations
First you can create an application cache library like so:

```csharp
var binarySerializer = new ProtobufBinarySerializer();
var cacheProvider = new BasicCacheProvider(new Dictionary<int, ICacheService>()
{
	{1, new StackExchangeRedisService("localhost:6379") },
	{2, new StackExchangeRedisService("localhost:6380") }
}, binarySerializer);
```
First dictionary defines services to be concerned. Their first parameters are identifiers of services. 
And second parameter is given binary serializer for internal serialize/deserialize operations.
cacheProvider object most of the time is singleton per application lifestyle. 

## Execute Function / Set&Get
Then for caching a function return value, you can call ExecuteCached method. By doing this, you both calling function and caching it with given key:

```csharp
var result = cacheProvider.ExecuteCached(new CacheKey("SomeCacheKey"), 
	() => Guid.NewGuid(), TimeSpan.FromSeconds(10));
```
## Get Only
For Get purpose there is also Get function with type declaration:
```csharp
var result = cacheProvider.Get<Guid>(new CacheKey("SomeCacheKey"));
```
## Delete
For deleting something from cache you can use ExecuteCached with CacheRule parameter:
```csharp
cacheProvider.ExecuteCached(new CacheKey("SomeCacheKey"), 
	() => Guid.NewGuid(), CacheRule.Delete());
```
## Bypass and Re-Execute Function Before Timeout Happens
Sometimes you need to re-execute same function and re-set cache value for same key. In this case we can use ExecuteCached like this:

```csharp
var result = cacheProvider.ExecuteCached(new CacheKey("SomeCacheKey"), 
	() => Guid.NewGuid(), TimeSpan.FromSeconds(10), true);  
```
Fourth parameter is bypassing the cache and executing given function, then adding it to the cache again.

## Explicitly using services
Since this is a basic library, I sometimes need to use the service under the hood. So I can call these services like this:
```
var cacheServices = cacheProvider.GetServices();
```
This operation returns ``IEnumerable<ICacheService>`` typed object.

## Bulk Delete
You can bulk delete keys from each service like this:
```csharp
foreach(var svc in cacheProvider.GetServices())
{
  svc.ClearCache(new CacheClearQuery(pattern: "SomeRepository::*"));
}
```
In StackExchangeRedisService implementation, I use Scan and Delete each key.

## Using Real Client in code
If you want more functions than this facade library, then you can use real implementation that works under the hood. 
For example, StackExchange.Redis library ConnectionMultiplexer is working under the StackExchangeRedisService implementation.
So when you need ConnectionMultiplexer actually, you can use it like this:
```csharp
var seRedisSvc = cacheProvider.GetServices()
    .First(svc => 
		svc is CacheServiceBase<ConnectionMultiplexer>) as CacheServiceBase<ConnectionMultiplexer>;
seRedisSvc.CacheEndpoint.GetDatabase().Ping();
```
Here, ICacheService implementation StackExchangeRedisService is also concrete class of CacheServiceBase<ConnectionMultiplexer>. This base class contains CacheEndpoint property as generic variable (in this case ConnectionMultiplexer).

# References
* [StackExhange.Redis](https://github.com/StackExchange/StackExchange.Redis)
* [Protobuf-net](https://github.com/mgravell/protobuf-net)

