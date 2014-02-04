using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixPack.Assets;

namespace SixPack
{
    public static class SixPackHelpers
    {
        /// <summary>
        /// Joins the appropriate content (minified or unminified) to return a single string
        /// </summary>
        /// <param name="assets">A collection of assets that has already gone through the minification process</param>
        /// <returns></returns>
        public static string JoinAssets(IEnumerable<Asset> assets)
        {
            var _result = new StringBuilder();
            var _errors = new StringBuilder();

            foreach (var asset in assets.OrderBy(a => a.SortOrder))
            {
                if (!String.IsNullOrWhiteSpace(asset.ErrorContent))
                {
                    _errors.Append(asset.ErrorContent);
                    _errors.AppendLine();
                }

                switch (asset.Status)
                {
                    case AssetStatus.MinifiedWithoutComments:
                        _result.Append(asset.MinifiedContent);
                        break;
                    case AssetStatus.MinifiedWithComments:
                        _result.Append(asset.MinifiedContent);
                        _result.AppendLine();
                        break;
                    case AssetStatus.NotMinified:
                        _result.Append(asset.Content);
                        _result.AppendLine();
                        break;
                    default:
                        if (!String.IsNullOrWhiteSpace(asset.MinifiedContent))
                        {
                            _result.Append(asset.MinifiedContent);
                            _result.AppendLine();
                        }
                        else if (!String.IsNullOrWhiteSpace(asset.Content))
                        {
                            _result.Append(asset.Content);
                            _result.AppendLine();
                        }
                        break;
                }
            }

            return _errors.Append(_result.ToString()).ToString();
        }

        /// <summary>
        /// Prepends the javascript with error information and returns the unminified javascript
        /// </summary>
        /// <param name="asset">An Asset with at least the Url and Content already set (i.e. http://myapp.com/scripts/main.js) </param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static Asset GetUnminifiedJsOnError(Asset asset, string errorMsg)
        {
            var _str = new StringBuilder();
            _str.AppendLine();
            _str.AppendLine("/* " + errorMsg + " */");
            _str.AppendLine();
            _str.Append(asset.Content);

            asset.Status = AssetStatus.MinificationFailed;
            asset.MinifiedContent = asset.Content;
            asset.ErrorContent = _str.ToString();

            return asset;
        }
    }
}
