using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixPack.Assets;
using SixPack.Consumers;

namespace SixPack.Minifiers
{
    public class BundleOnlyMinifier : IMinifier
    {
        protected static IConsumer _consumer;
        
        public BundleOnlyMinifier(IConsumer consumer)
        {
            if (consumer == null)
                throw new ArgumentException("The consumer parameter that implements IConsumer is required.", "consumer");

            _consumer = consumer;
        }

        public async Task<IEnumerable<IAsset>> Minify(IEnumerable<IAsset> assets)
        {
            var _tasks = new HashSet<Task<IAsset>> { };

            foreach (var asset in assets)
            {
                _tasks.Add(_consumer.GetScriptAsync(asset));
            }

            return await Task.WhenAll(_tasks.ToArray());  
        }

        ///// <summary>
        ///// Use Google's Closure Complier service to minify the javascript code
        ///// </summary>
        ///// <param name="jsCode"></param>
        ///// <returns></returns>
        //protected virtual IFileResult MinifyScript(string jsCode)
        //{


        //    //Minifier minifier = new Minifier();
        //    //string content = minifier.MinifyJavaScript(response.Content, new CodeSettings
        //    //{
        //    //    EvalTreatment = EvalTreatment.MakeImmediateSafe,
        //    //    PreserveImportantComments = false
        //    //});
        //    //if (minifier.ErrorList.Count > 0)
        //    //{
        //    //    BetterMinify.GenerateErrorResponse(response, minifier.ErrorList);
        //    //}

        //    //using (HttpClient client = new HttpClient())
        //    //{
        //    //    var _closureObj = new ClosureCompilerObject();

        //    //    // todo: the js will be too long, we need to use code_url instead
        //    //    _closureObj.js_code = jsCode;
        //    //    var response = client.PostAsync("http://closure-compiler.appspot.com/compile", new FormUrlEncodedContent(_closureObj.AsNameValueCollection())).Result;

        //    //    if (response.IsSuccessStatusCode)
        //    //    {
        //    //        string _code = response.Content.ReadAsStringAsync().Result;
        //    //        if (_code.StartsWith("Error"))
        //    //        {
        //    //            return GetUnminifiedJsOnError(jsCode, _code);
        //    //        }

        //    //        return new FileResultVw
        //    //        {
        //    //            Succeeded = true,
        //    //            FileContent = _code
        //    //        };
        //    //    }
        //    //    else
        //    //    {
        //    //        return new FileResultVw
        //    //        {
        //    //            Succeeded = false,
        //    //            FileContent = jsCode
        //    //        };
        //    //    }
        //    //}
        //}
    }
}
