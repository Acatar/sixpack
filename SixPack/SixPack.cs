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
        }

        public async Task<Bundle> GetBundleContent(string bundleName, string iMinifierFactoryName, ICollection<string> filePathArray = null, bool useCache = true)
        {
            if (String.IsNullOrWhiteSpace(bundleName))
                throw new ArgumentException("Bundles have to be named. The bundleName parameter cannot be null or empty.", "bundle.Name");

            if (useCache && _cacheProvider.Exists(bundleName, group: _cacheGroup))
                return _cacheProvider.Get<Bundle>(bundleName, group: _cacheGroup);

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
                _cacheProvider.Set<Bundle>(_bundle.Name, _bundle, group: _cacheGroup);

            return _bundle;
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
    }
}
