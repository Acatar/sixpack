using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixPack
{
    public interface ISixPack
    {
        /// <summary>
        /// Gets all of the javascript files that are located at the paths given in the filePathArray, and bundles their code 
        /// into a single string.  The code is minifed.  The files are concatenated in the order 
        /// that they are aligned in the filePathArray. They resulting output is cached, so it can be served from memory on all 
        /// future requests.
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="iMinifierFactoryName">The name of the IMinifier factory that is stored in SixPackServiceLocators.IMinifierCreationFacotries</param>
        /// <param name="filePathArray">HashSet of type string: a list of HTTP paths to the files you wish to bundle</param>
        /// <returns>string: the assets, minified and joined into a single string</returns>
        Task<string> GetBundle(string bundleName, string iMinifierFactoryName, ICollection<string> filePathArray = null);
    }
}
