using Microsoft.Ajax.Utilities;
using SixPack.Assets;
using SixPack.Consumers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixPack.Minifiers.BetterMinifyJS
{
    public sealed class BetterMinifier : IMinifier
    {
        protected static IConsumer _consumer;

        public BetterMinifier(IConsumer consumer)
        {
            if (consumer == null)
                // TODO: move this text to locale
                throw new ArgumentException("The consumer parameter that implements IConsumer is required.", "consumer");

            _consumer = consumer;
        }

        public async Task<IEnumerable<Asset>> Minify(IEnumerable<Asset> assets)
        {
            var _tasks = new HashSet<Task<Asset>> { };

            foreach (var _asset in assets)
            {
                _tasks.Add(_consumer.GetScriptAsync(_asset));
            }

            Asset[] _loadedAssets =  await Task.WhenAll(_tasks.ToArray());

            foreach(var _asset in _loadedAssets)
            {
                if (_asset.Status == AssetStatus.NotMinified)
                {
                    try
                    {
                        _asset.MinifiedContent = Process(_asset.Content);
                        _asset.Status = AssetStatus.MinifiedWithComments;
                    }
                    catch (Exception ex)
                    {
                        _asset.MinifiedContent = null;
                        _asset.Status = AssetStatus.MinificationFailed;
                    }
                }
            }

            return _loadedAssets;
        }

        private static string Process(String content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            Minifier minifier = new Minifier();
            string _result;
            string _minified = minifier.MinifyJavaScript(content, new CodeSettings
            {
                EvalTreatment = EvalTreatment.MakeImmediateSafe,
                PreserveImportantComments = false
            });

            if (minifier.ErrorList.Count > 0)
            {
                StringBuilder _errorList = new StringBuilder("Errors encountered during minification: " + System.Environment.NewLine);
                foreach (object error in minifier.ErrorList)
                {
                    _errorList.Append(error.ToString()).Append(System.Environment.NewLine);
                }
                
                throw new MinificationException(_errorList.ToString());
            }
            else
            {
                _result = _minified;
            }

            return _result;
        }
    }
}
