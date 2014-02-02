using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SixPack.Assets;

namespace SixPack.HttpHandlers
{
    public class DefaultHandler : IHttpHandler
    {
        public static ISixPack _sixPack { private get; set; }

        public void ProcessRequest(HttpContext context)
        {
            var _preprocessedBundle = context.GetFileArrayFromQueryString();
            var _bundle = Task.Run<Bundle>(() => _sixPack.GetBundleContent(_preprocessedBundle.Name, _preprocessedBundle.ContentExtension, _preprocessedBundle.FilePathArray)).Result;

            // Write the response out to the client
            context.Response.StatusCode = 200;
            context.Response.ContentType = _bundle.MimeType;
            context.Response.Write(_bundle.Content);
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
