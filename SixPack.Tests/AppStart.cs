using LightInject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SixPack.Assets;
using SixPack.Cache;
using SixPack.Consumers;
using SixPack.Locale;
using SixPack.MemoryCacheProvider;
using SixPack.Minifiers;

namespace SixPack.Tests
{
    [TestClass]
    public class Startup
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            var _container = new ServiceContainer();

            _container.Register<ISixPack, SixPack>();
            _container.Register<IConsumer, Consumer>();
            _container.Register<IMinifier, BundleOnlyMinifier>();
            _container.Register<ILocale, Locale_En>();
            _container.Register<ICacheProvider, MemoryCacheProviderImplementation>();

            SixPackServiceLocators.IMinifierFacotries.Add(Constants.JsMinifierName, () => _container.GetInstance<IMinifier>());
            //SixPackServiceLocators.IConsumerCreationFactory = () => _container.GetInstance<IConsumer>();

            _BaseTest._sixpack = _container.GetInstance<ISixPack>();
            
            //alternatively:
            //_BaseTest._sixpack = new SixPack(new MemoryCacheProviderImplementation(), new Locale_En());
        }
    }
}
