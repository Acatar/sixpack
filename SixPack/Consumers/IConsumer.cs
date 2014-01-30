using System.Collections.Generic;
using System.Threading.Tasks;
using SixPack.Assets;

namespace SixPack.Consumers
{
    public interface IConsumer
    {
        /// <summary>
        /// Joins the appropriate content (minified or unminified) to return a single string
        /// </summary>
        /// <param name="assets">A collection of assets that has already gone through the minification process</param>
        /// <returns></returns>
        string JoinAssets(IEnumerable<IAsset> assets);

        /// <summary>
        /// Gets a script from a web resource, asynchronously
        /// </summary>
        /// <param name="asset">An Asset with at least the Url and SortOrder already set (i.e. http://myapp.com/scripts/main.js) </param>
        /// <returns>IAsset: an asset with content and status information</returns>
        Task<IAsset> GetScriptAsync(IAsset asset);

        /// <summary>
        /// Gets a script from a web resource, synchronously
        /// </summary>
        /// <param name="asset">An Asset with at least the Url and SortOrder already set (i.e. http://myapp.com/scripts/main.js) </param>
        /// <returns>IAsset: an asset with content and status information</returns>
        IAsset GetScript(IAsset asset);

        /// <summary>
        /// Prepends the javascript with error information and returns the unminified javascript
        /// </summary>
        /// <param name="asset">An Asset with at least the Url and Content already set (i.e. http://myapp.com/scripts/main.js) </param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        IAsset GetUnminifiedJsOnError(IAsset asset, string errorMsg);

    }
}
