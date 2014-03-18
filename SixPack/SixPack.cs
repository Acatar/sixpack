using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixPack.Assets;
using SixPack.Cache;
using SixPack.Consumers;
using SixPack.Locale;
using SixPack.Minifiers;

namespace SixPack
{
    public class SixPack : ISixPack
    {
        protected static IMinifier _minifier;
        protected static ICacheProvider _cacheProvider;
        protected static ILocale _locale;
        protected static SixPackServiceLocators _serviceLocators;

        public SixPack(ICacheProvider cacheProvider, ILocale locale)
        {
            if (locale != null)
                _locale = locale;
            else
                _locale = new Locale_En();

            if (cacheProvider == null)
                throw new ArgumentException(locale.CacheProviderNotImplemented, "cacheProvider");

            _cacheProvider = cacheProvider;
        }

        public async Task<Bundle> GetBundleContent(string bundleName, string iMinifierFactoryName, ICollection<string> filePathArray = null, bool useCache = true)
        {
            if (String.IsNullOrWhiteSpace(bundleName))
                throw new ArgumentException("Bundles have to be named. The bundleName parameter cannot be null or empty.", "bundle.Name");

            string _bundleCacheKey = GetBundleCacheKey(bundleName);

            if (useCache && _cacheProvider.Exists(_bundleCacheKey))
                return _cacheProvider.Get<Bundle>(_bundleCacheKey);

            if (filePathArray == null || !filePathArray.Any())
                return new Bundle { Name = bundleName, Content = "/* No files were found to bundle for " + bundleName + ". */" };

            var _firstFile = filePathArray.FirstOrDefault();
            string _contentExtension = _firstFile.Contains('.') ? _firstFile.Substring(_firstFile.LastIndexOf('.') + 1) : null;

            if (String.IsNullOrWhiteSpace(iMinifierFactoryName) && String.IsNullOrWhiteSpace(_contentExtension))
                throw new ArgumentException("iMinifierFactoryName must be defined", "iMinifierFactoryName");
            
            if (String.IsNullOrWhiteSpace(iMinifierFactoryName))
                iMinifierFactoryName = _contentExtension;

            var _assets = await GetMinifiedAssets(FileArrayToAssets(filePathArray), iMinifierFactoryName);

            var _bundle = new Bundle { 
                Name = bundleName,
                Content = SixPackHelpers.JoinAssets(_assets),
                ContentExtension = _contentExtension,
                FilePathArray = filePathArray
            };

            if (useCache) 
            {
                _cacheProvider.Set<Bundle>(_bundleCacheKey, _bundle);
                _cacheProvider.Set<LastModified>(GetLastModifiedCacheKey(_bundle.Name), new LastModified 
                    { 
                        BundleName = _bundle.Name,
                        TimeUtcLastModified = DateTime.UtcNow
                    });
            }

            return _bundle;
        }

        /// <summary>
        /// Get the time that the bundle was cached (the last modified time) in an async call
        /// </summary>
        /// <param name="bundleName">the name of the bundle to check the last modified time for</param>
        /// <returns>The UTC Time that the bundle was cached, if it is found, otherwise UTCNow</returns>
        public async Task<DateTime> GetLastModifiedTimeAsync(string bundleName)
        {
            return await Task.Run<DateTime>(() => GetLastModifiedTime(bundleName));
        }

        /// <summary>
        /// Get the time that the bundle was cached (the last modified time).
        /// </summary>
        /// <param name="bundleName">the name of the bundle to check the last modified time for</param>
        /// <returns>The UTC Time that the bundle was cached, if it is found, otherwise UTCNow</returns>
        public DateTime GetLastModifiedTime(string bundleName) 
        {
            var _cacheKey = GetLastModifiedCacheKey(bundleName);
            var _result = _cacheProvider.Get<LastModified>(_cacheKey);

            return _result != null ? _result.TimeUtcLastModified : DateTime.UtcNow;
        }

        protected virtual IEnumerable<Asset> FileArrayToAssets(ICollection<string> filePathArray) 
        {
            int _order = 1;

            foreach (var item in filePathArray)
            {
                yield return new Asset { 
                    Url = item,
                    Status = AssetStatus.NotStarted,
                    SortOrder = _order
                };

                _order++;
            }
        }

        /// <summary>
        /// Gets a script from a web resource, asynchronously
        /// </summary>
        /// <param name="path">the path to the web resource (i.e. http://myapp.com/scripts/main.js)</param>
        /// <returns>FileResultVw: the code in a string and a success bool</returns>
        protected virtual async Task<IEnumerable<Asset>> GetMinifiedAssets(IEnumerable<Asset> assets, string minifierFactoryName)
        {
            if(_serviceLocators == null)
                _serviceLocators = new SixPackServiceLocators();

            if (!_serviceLocators.MinifierFactoryExists(minifierFactoryName))
                throw new NullReferenceException(String.Format("A minifier factory with the name, {0}, does not exist in SixPackServiceLocators.IMinifierFactories.", minifierFactoryName));

            if (_minifier == null)
                _minifier = _serviceLocators.GetMinifierInstance(minifierFactoryName);

            return await _minifier.Minify(assets);
        }

        /// <summary>
        /// Gets the cache key for the bundle
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        protected static string GetBundleCacheKey(string bundleName)
        {
            return String.Format("sixpack::{0}", bundleName);
        }

        /// <summary>
        /// Gets the cache key for the bundle's last modified time
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        protected static string GetLastModifiedCacheKey(string bundleName)
        {
            return String.Format("sixpack::{0}::last_modified", bundleName);
        }
    }
}
