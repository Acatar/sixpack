using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Extensions;
using SixPack.Minifiers;
using SixPack.Consumers;
using SixPack.Examples.Modules;
using SixPack.Locale;

[assembly: OwinStartup(typeof(SixPack.Examples.Startup))]
namespace SixPack.Examples
{
    /// <summary>
    /// OWIN Startup
    /// </summary>
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
            //app.UseStageMarker(PipelineStage.MapHandler);

            BundlesModule._sixPack = new SixPack(new MemoryCacheProvider.MemoryCacheProviderImplementation(), new Locale_En());
            SixPackServiceLocators.IMinifierFactories.Add("js", () => new BundleOnlyMinifier(new Consumer()));
        }
    }
}