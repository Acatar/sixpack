using System.Collections.Generic;
using System.Threading.Tasks;
using SixPack.Assets;

namespace SixPack.Minifiers
{
    public interface IMinifier
    {
        /// <summary>
        /// Minifies a collection of assets (i.e. javascript, css)
        /// <param name="assets">A collection of assets</param>
        /// <returns>The same collection that was passed in the assets parameter, having been processed</returns>
        Task<IEnumerable<Asset>> Minify(IEnumerable<Asset> assets);
    }
}
