using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SixPack.Assets;

namespace SixPack.Consumers
{
    public class Consumer : IConsumer
    {
        public string JoinAssets(IEnumerable<IAsset> assets) 
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

            _errors.Append(_result.ToString());
            string _finalResult = _errors.ToString();

            return _finalResult;
        }

        public virtual async Task<IAsset> GetScriptAsync(IAsset asset)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var _response = await client.GetAsync(asset.Url);
                    if (_response.IsSuccessStatusCode)
                    {
                        string _code = await _response.Content.ReadAsStringAsync();
                        asset.Status = AssetStatus.NotMinified;
                        asset.Content = _code;

                        return asset;
                    }
                    else
                    {
                        throw new Exception("StatusCode did not indicate success (" + _response.StatusCode + ")");
                    }
                }
                catch (Exception e)
                {
                    StringBuilder _strBuilder = new StringBuilder();
                    _strBuilder.AppendLine("/* Error loading path: " + asset.Url + ".  " + e.Message + " */");
                    _strBuilder.AppendLine();

                    asset.Status = AssetStatus.NotFound;
                    asset.ErrorContent = _strBuilder.ToString();

                    return asset;
                }
            }
        }

        public virtual IAsset GetScript(IAsset asset)
        {
            using (var client = new WebClient())
            {
                try
                {
                    using (var _streamReader = new StreamReader(client.OpenRead(asset.Url)))
                    {
                        asset.Content = _streamReader.ReadToEnd();
                        asset.Status = AssetStatus.NotMinified;

                        return asset;
                    }
                }
                catch (Exception e)
                {
                    StringBuilder _strBuilder = new StringBuilder();
                    _strBuilder.AppendLine("/* Error loading path: " + asset.Url + ".  " + e.Message + " */");
                    _strBuilder.AppendLine();

                    asset.Status = AssetStatus.NotFound;
                    asset.ErrorContent = _strBuilder.ToString();

                    return asset;
                }
            }
        }

        public virtual IAsset GetUnminifiedJsOnError(IAsset asset, string errorMsg)
        {
            var _str = new StringBuilder();
            _str.AppendLine();
            //_str.AppendLine("/* Error minifying path: " + path + " */");
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
