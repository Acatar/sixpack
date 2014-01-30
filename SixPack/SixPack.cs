using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixPack.Assets;
using SixPack.Cache;
using SixPack.Locale;
using SixPack.Minifiers;

namespace SixPack
{
    public class SixPack : ISixPack
    {
        protected static string _cacheGroup = "sixpack::";
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
            _serviceLocators = new SixPackServiceLocators();
        }

        public string CreateBundle(string bundleName, ICollection<string> filePathArray)
        {
            return Task.Run<string>(() => GetBundle(bundleName, filePathArray)).Result;
        }

        public async Task<string> GetBundle(string bundleName, ICollection<string> filePathArray = null)
        {
            if (String.IsNullOrWhiteSpace(bundleName))
            {
                throw new NullReferenceException("Bundles have to be named. The bundleName parameter cannot be null or empty.");
            }

            if (_cacheProvider.Exists(bundleName, group: _cacheGroup))
            {
                return _cacheProvider.Get<string>(bundleName, group: _cacheGroup);
            }

            if (filePathArray == null || !filePathArray.Any())
            {
                return "/* No files were found to bundle for " + bundleName + ". */";
            }

            var _assets = await GetMinifiedAssets(FileArrayToAssets(filePathArray));
            var _result = _serviceLocators.GetConsumerInstance().JoinAssets(_assets);
            _cacheProvider.Set<string>(bundleName, _result, group: _cacheGroup);

            return _result;
        }

        protected virtual IEnumerable<IAsset> FileArrayToAssets(ICollection<string> filePathArray) 
        {
            int _order = 1;

            foreach (var item in filePathArray)
            {
                var _asset = _serviceLocators.GetAssetInstance();
                _asset.Url = item;
                _asset.Status = AssetStatus.NotStarted;
                _asset.SortOrder = _order;
                yield return _asset;

                _order++;
            }
        }

        /// <summary>
        /// Gets a script from a web resource, asynchronously
        /// </summary>
        /// <param name="path">the path to the web resource (i.e. http://myapp.com/scripts/main.js)</param>
        /// <returns>FileResultVw: the code in a string and a success bool</returns>
        protected virtual async Task<IEnumerable<IAsset>> GetMinifiedAssets(IEnumerable<IAsset> assets)
        {
            if (_minifier == null)
                _minifier = _serviceLocators.GetMinifierInstance();

            return await _minifier.Minify(assets);
        }
    }
}
