using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Grain.Extensions;
using SixPack.Assets;

namespace SixPack.HttpHandlers
{
    public static class ContextHelper
    {
        const string bundleParamName = "name";
        const string fileArrayParamName = "urls";
        const string baseUrlParamName = "baseUrl";

        public static Bundle GetFileArrayFromQueryString(this HttpContext context, string name = null) 
        {
            if (context == null || context.Request == null || context.Request.Url == null)
                throw new ArgumentException("The HttpContext, it's Request and the Request.Url must be present to get content from the query string.", "context");

            var _query = HttpUtility.ParseQueryString(context.Request.Url.Query.Substring(1));
            var _name = name != null ? name : _query[bundleParamName];

            if(String.IsNullOrWhiteSpace(_name))
                return null;

            var _filesString = _query[fileArrayParamName];
            var _files = String.IsNullOrWhiteSpace(_filesString) ? null : _filesString.ToList(',');
            var _firstFile = _files.FirstOrDefault();
            string _extension = null;

            if (_firstFile != null) {
                _extension = _firstFile.Substring(_firstFile.LastIndexOf('.') + 1);

                var _uri = context.Request.Url;
                var _basePath = _uri.Port == 80 || _uri.Port == 443 ? "http://" + _uri.Host + "/" : "http://" + _uri.Authority + "/";
                var _baseUrl = _query[baseUrlParamName];

                if (_baseUrl != null) 
                {
                    _baseUrl = _baseUrl.StartsWith("/") ? _baseUrl.Substring(1) : _baseUrl;
                    _baseUrl = _baseUrl.EndsWith("/") ? _baseUrl : _baseUrl + "/";                
                }

                _files = ProcessUrls(_files, _basePath + _baseUrl).ToList();
            }

            return new Bundle {
                Name = _name, 
                FilePathArray = _files,
                ContentExtension = _extension
            };
        }

        private static IEnumerable<string> ProcessUrls(List<string> filePathArray, string baseUrl) 
        {
            foreach (var item in filePathArray) 
            {
                if (item.StartsWith("http"))
                    yield return item;

                if (item.StartsWith("//"))
                    yield return "http:" + item;

                yield return item.StartsWith("/") ? baseUrl + item.Substring(1) : baseUrl + item;
            }
        }
    }
}
