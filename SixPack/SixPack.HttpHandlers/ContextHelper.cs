using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Grain.Extensions;

namespace SixPack.HttpHandlers
{
    public static class ContextHelper
    {
        const string bundleParamName = "name";
        const string fileArrayParamName = "urls";

        public static IDictionary<string, ICollection<string>> GetFileArrayFromQueryString(HttpContext context) 
        {
            var _query = HttpUtility.ParseQueryString(context.Request.Url.Query.Substring(1));
            var _bundle = new HashSet<string> { };
            var _files = _query[fileArrayParamName].ToHashSet(',');

            foreach (var item in _files) 
            {
                _bundle.Add(item);
            }

            return new Dictionary<string, ICollection<string>> {
                { _query[bundleParamName], _bundle }
            };
        }
    }
}
