using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SixPack.Assets;
using SixPack.HttpHandlers;
using SixPack.Locale;

namespace SixPack.Examples.Handlers
{
    public class DefaultHandler : IHttpHandler
    {
        public static ISixPack _sixPack { private get; set; }

        public void ProcessRequest(HttpContext context)
        {
            _sixPack = new SixPack(new MemoryCacheProvider.MemoryCacheProviderImplementation(), new Locale_En());
            var _preprocessedBundle = context.GetFileArrayFromQueryString();
            var _bundle = Task.Run<Bundle>(() => _sixPack.GetBundleContent(_preprocessedBundle.Name, _preprocessedBundle.ContentExtension, _preprocessedBundle.FilePathArray));
            var _result = _bundle.Result;

            // Write the response out to the client
            context.Response.StatusCode = 200;
            context.Response.ContentType = _result.MimeType;
            context.Response.Write(_result.Content);
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}