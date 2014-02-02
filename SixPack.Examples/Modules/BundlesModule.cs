using System.Web;
using Nancy;
using SixPack.HttpHandlers;
using SixPack.Locale;
using SixPack.NancyExtensions.Responses;

namespace SixPack.Examples.Modules
{
    public class BundlesModule : NancyModule
    {
        public static ISixPack _sixPack { private get; set; }

        public BundlesModule() 
        {
            // i.e. http://localhost:12008/bundles/core?urls=scripts/jquery-2.1.0.min.js,scripts/home.js
            Get["/bundles/{name}", true] = async (x, cancellationToken) =>
            {
                string _name = x.name;
                var _preprocessedBundle = HttpContext.Current.GetFileArrayFromQueryString(name: _name);
                var _bundle = await _sixPack.GetBundleContent(_preprocessedBundle.Name, _preprocessedBundle.ContentExtension, _preprocessedBundle.FilePathArray); //Task.Run<Bundle>(() => );

                return new JsResponse(_bundle.Content, _bundle.MimeType);
            };        
        }
    }
}