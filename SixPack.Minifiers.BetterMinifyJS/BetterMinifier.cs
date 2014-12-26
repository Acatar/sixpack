using Microsoft.Ajax.Utilities;
using SixPack.Assets;
using SixPack.Consumers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixPack.Minifiers.BetterMinifyJS
{
    public sealed class BetterMinifier : IMinifier
    {
        protected static IConsumer _consumer;

        public BetterMinifier(IConsumer consumer)
        {
            if (consumer == null)
                // TODO: move this text to locale
                throw new ArgumentException("The consumer parameter that implements IConsumer is required.", "consumer");

            _consumer = consumer;
        }

        public async Task<IEnumerable<Asset>> Minify(IEnumerable<Asset> assets)
        {
            var _tasks = new HashSet<Task<Asset>> { };

            foreach (var asset in assets)
            {
                _tasks.Add(_consumer.GetScriptAsync(asset));
            }

            return await Task.WhenAll(_tasks.ToArray());
        }

        private static string Process(String content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            Minifier minifier = new Minifier();
            string _result;
            string _minified = minifier.MinifyJavaScript(content, new CodeSettings
            {
                EvalTreatment = EvalTreatment.MakeImmediateSafe,
                PreserveImportantComments = false
            });

            if (minifier.ErrorList.Count > 0)
            {
                _result = GenerateErrorResponse(content, minifier.ErrorList);
            }
            else
            {
                _result = _minified;
            }

            return _result;
        }

        private static string GenerateErrorResponse(string content, IEnumerable<object> errors)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("/* ");
            stringBuilder.Append("Minification Error").Append("\r\n");
            foreach (object current in errors)
            {
                stringBuilder.Append(current.ToString()).Append("\r\n");
            }
            stringBuilder.Append(" */\r\n");
            stringBuilder.Append(content);
            return stringBuilder.ToString();
        }
    }
}
