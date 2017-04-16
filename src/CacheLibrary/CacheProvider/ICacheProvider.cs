using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CacheLibrary.CacheHelper;
using CacheLibrary.CacheService;

namespace CacheLibrary.CacheProvider
{
    /// <summary>
    /// Represents basic functions of a CacheProvider
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Executes given function and assigns its value to given key parameter. Returns executed result of given function.
        /// </summary>
        /// <typeparam name="T">Result object</typeparam>
        /// <param name="cacheKey">Cache key parameter for storing it in cache server</param>
        /// <param name="cacheFunc">Function for executing and storing in cache server</param>
        /// <returns>Returns the result of function</returns>
        T ExecuteCached<T>(CacheKey cacheKey, Func<T> cacheFunc);

        /// <summary>
        /// Executes given function and assigns its value to given key parameter. Returns executed result of given function.
        /// </summary>
        /// <typeparam name="T">Result object</typeparam>
        /// <param name="cacheKey">Cache key parameter for storing it in cache server</param>
        /// <param name="cacheFunc">Function for executing and storing in cache server</param>
        /// <param name="expiresIn">Defines how much time later cache will be invalidated by server</param>
        /// <returns>Returns the result of function</returns>
        T ExecuteCached<T>(CacheKey cacheKey, Func<T> cacheFunc, TimeSpan expiresIn);

        /// <summary>
        /// Executes given function and assigns its value to given key parameter. Returns executed result of given function.
        /// </summary>
        /// <typeparam name="T">Result object</typeparam>
        /// <param name="cacheKey">Cache key parameter for storing it in cache server</param>
        /// <param name="cacheFunc">Function for executing and storing in cache server</param>
        /// <param name="expiresIn">Defines how much time later cache will be invalidated by server</param>
        /// <param name="bypassCache">If it is true then cacheFunc function executes and sets its result even though there is a value of key in server. 
        /// Otherwise if there is a parameter, it instantly gets the value and returns without executing function</param>
        /// <returns>Returns the result of function</returns>
        T ExecuteCached<T>(CacheKey cacheKey, Func<T> cacheFunc, TimeSpan expiresIn, bool bypassCache);

        /// <summary>
        /// Executes given function and assigns its value to given key parameter. Returns executed result of given function.
        /// </summary>
        /// <typeparam name="T">Result object</typeparam>
        /// <param name="cacheKey">Cache key parameter for storing it in cache server</param>
        /// <param name="cacheFunc">Function for executing and storing in cache server</param>
        /// <param name="cacheRule">A ruleset for a caching operation</param>
        /// <returns>Returns the result of function</returns>
        T ExecuteCached<T>(CacheKey cacheKey, Func<T> cacheFunc, CacheRule cacheRule);

        /// <summary>
        /// Async version of ExecuteCached function. Executes given function and assigns its value to given key parameter. Returns executed result of given function.
        /// </summary>
        /// <typeparam name="T">Result object</typeparam>
        /// <param name="cacheKey">Cache key parameter for storing it in cache server</param>
        /// <param name="cacheFunc">Function for executing and storing in cache server</param>
        /// <returns>Returns the result of function</returns>
        Task<T> ExecuteCachedAsync<T>(CacheKey cacheKey, Func<T> cacheFunc);

        /// <summary>
        /// Async version of ExecuteCached function. Executes given function and assigns its value to given key parameter. Returns executed result of given function.
        /// </summary>
        /// <typeparam name="T">Result object</typeparam>
        /// <param name="cacheKey">Cache key parameter for storing it in cache server</param>
        /// <param name="cacheFunc">Function for executing and storing in cache server</param>
        /// <param name="expiresIn">Defines how much time later cache will be invalidated by server</param>
        /// <returns>Returns the result of function</returns>
        Task<T> ExecuteCachedAsync<T>(CacheKey cacheKey, Func<T> cacheFunc, TimeSpan expiresIn);

        /// <summary>
        /// Async version of ExecuteCached function. Executes given function and assigns its value to given key parameter. Returns executed result of given function.
        /// </summary>
        /// <typeparam name="T">Result object</typeparam>
        /// <param name="cacheKey">Cache key parameter for storing it in cache server</param>
        /// <param name="cacheFunc">Function for executing and storing in cache server</param>
        /// <param name="expiresIn">Defines how much time later cache will be invalidated by server</param>
        /// <param name="bypassCache">If it is true then cacheFunc function executes and sets its result even though there is a value of key in server. 
        /// Otherwise if there is a parameter, it instantly gets the value and returns without executing function</param>
        /// <returns>Returns the result of function</returns>
        Task<T> ExecuteCachedAsync<T>(CacheKey cacheKey, Func<T> cacheFunc, TimeSpan expiresIn, bool bypassCache);

        /// <summary>
        /// Async version of ExecuteCached function. Executes given function and assigns its value to given key parameter. Returns executed result of given function.
        /// </summary>
        /// <typeparam name="T">Result object</typeparam>
        /// <param name="cacheKey">Cache key parameter for storing it in cache server</param>
        /// <param name="cacheFunc">Function for executing and storing in cache server</param>
        /// <param name="cacheRule">A ruleset for a caching operation</param>
        /// <returns>Returns the result of function</returns>
        Task<T> ExecuteCachedAsync<T>(CacheKey cacheKey, Func<T> cacheFunc, CacheRule cacheRule);

        /// <summary>
        /// Gets the cache key from server
        /// </summary>
        /// <typeparam name="T">Return value object</typeparam>
        /// <param name="cacheKey"></param>
        /// <returns>Returns the value of cache key from server in object form</returns>
        T Get<T>(CacheKey cacheKey);

        /// <summary>
        /// Async version of Get function. Gets the cache key from server
        /// </summary>
        /// <typeparam name="T">Return value object</typeparam>
        /// <param name="cacheKey"></param>
        /// <returns>Returns the value of cache key from server in object form</returns>
        Task<T> GetAsync<T>(CacheKey cacheKey);

        /// <summary>
        /// Gets the registered services from current provider
        /// </summary>
        /// <returns>Returns the registered cache services</returns>
        IEnumerable<ICacheService> GetServices();
    }
}