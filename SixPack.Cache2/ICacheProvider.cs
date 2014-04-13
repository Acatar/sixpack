using System;

namespace SixPack.Cache
{
    public interface ICacheProvider
    {
        /// <summary>
        /// Checks to see if given grouped key exists in the cache
        /// </summary>
        /// <param name="key">the unique key to check for</param>
        /// <param name="group">the data group this key belongs to</param>
        /// <returns>true, if the key exists in the cache, otherwise false</returns>
        bool Exists(string key, string group = "");

        /// <summary>
        /// Sets data in the cache, by the given key and with the given settings.  If the item already exists in the cache, 
        /// it is overwritten.  Otherwise it is added.
        /// </summary>
        /// <typeparam name="T">the type of data being set</typeparam>
        /// <param name="key">the unique key for the item being cached</param>
        /// <param name="data">the data that is being cached</param>
        /// <param name="group">the data group the cached item belongs to</param>
        /// <param name="expiresIn">the duration that this item will persist in the cache</param>
        /// <returns>true if the item was cached successfully. otherwise false.</returns>
        bool Set<T>(string key, T data, string group = "", TimeSpan? expiresIn = null);

        /// <summary>
        /// Gets data from the cache, by the given key.
        /// </summary>
        /// <param name="key">the unique key for the cached item</param>
        /// <param name="group">(optional) the data group the cached item belongs to</param>
        /// <param name="expiresIn">(optional) a new duration to persist this item in the cache (i.e. touch)</param>
        /// <returns>the cached item as an object</returns>
        object Get(string key, string group = "", TimeSpan? expiresIn = null);

        /// <summary>
        /// Gets data from the cache, by the given key.
        /// </summary>
        /// <typeparam name="T">the type of the requested data (i.e. the type it will be deserialized or cast as)</typeparam>
        /// <param name="key">the unique key for the cached item</param>
        /// <param name="group">(optional) the data group the cached item belongs to</param>
        /// <param name="expiresIn">(optional) a new duration to persist this item in the cache (i.e. touch) </param>
        /// <returns>the cached item as T</returns>
        T Get<T>(string key, string group = "", TimeSpan? expiresIn = null) where T : class;
    }
}
