using System;
using System.Runtime.Caching;
using SixPack.Cache;

namespace SixPack.MemoryCacheProvider
{
    public class MemoryCacheProviderImplementation : ICacheProvider
    {
        ObjectCache _cache = MemoryCache.Default;

        protected virtual string GetCacheKey(string key, string group = "") 
        {
            return group + key;
        }

        public bool Exists(string key, string group = "")
        {
            return _cache.Contains(GetCacheKey(key, group));
        }

        public bool Set<T>(string key, T data, string group = "", TimeSpan? expiresIn = null)
        {
            var _policy = new CacheItemPolicy();

            if(expiresIn.HasValue)
                _policy.SlidingExpiration = expiresIn.Value;

            _cache.Set(GetCacheKey(key, group), data, _policy);
            return true;
        }

        /// <summary>
        /// Gets data from the cache, by the given key.
        /// </summary>
        /// <param name="key">the unique key for the cached item</param>
        /// <param name="group">(optional) the data group the cached item belongs to</param>
        /// <param name="expiresIn">NOT SUPPORTED</param>
        /// <returns>the cached item as an object</returns>
        public object Get(string key, string group = "", TimeSpan? expiresIn = null)
        {
            return _cache.Get(GetCacheKey(key, group));
        }

        /// <summary>
        /// Gets data from the cache, by the given key.
        /// </summary>
        /// <typeparam name="T">the type of the requested data (i.e. the type it will be deserialized or cast as)</typeparam>
        /// <param name="key">the unique key for the cached item</param>
        /// <param name="group">(optional) the data group the cached item belongs to</param>
        /// <param name="expiresIn">NOT SUPPORTED</param>
        /// <returns>the cached item as T</returns>
        public T Get<T>(string key, string group = "", TimeSpan? expiresIn = null) where T : class
        {
            return Get(key, group) as T;
        }
    }
}
