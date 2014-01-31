using System.Collections.Generic;
using System.Threading.Tasks;
using SixPack.Assets;

namespace SixPack.Consumers
{
    public interface IConsumer
    {
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

    }
}
