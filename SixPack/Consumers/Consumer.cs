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
                        // TODO: Move text to locale
                        throw new Exception("StatusCode did not indicate success (" + _response.StatusCode + ")");
                    }
                }
                catch (Exception e)
                {
                    return AddErrorContent(asset, e);
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
                    return AddErrorContent(asset, e);
                }
            }
        }

        /// <summary>
        /// Adds information about the exception that was encountered to the Asset
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private IAsset AddErrorContent(IAsset asset, Exception e)
        {
            StringBuilder _strBuilder = new StringBuilder();
            // TODO: Move text to locale
            _strBuilder.AppendLine("/* Error loading path: " + asset.Url + ".  " + e.Message + " */");
            _strBuilder.AppendLine();

            asset.Status = AssetStatus.NotFound;
            asset.ErrorContent = _strBuilder.ToString();

            return asset;
        }
    }
}
