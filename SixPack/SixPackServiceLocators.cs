using System;
using SixPack.Assets;
using SixPack.Consumers;
using SixPack.Minifiers;

namespace SixPack
{
    /// <summary>
    /// Using the Service Locator Pattern, provides the factories that are used to support (lazy) property injection in SixPack.
    /// All Get's in this class should supply a Local Default, if the correlating factory is not defined.
    /// 
    /// See SixPack.Tests.AppStart.cs for examples of how to configure these
    /// </summary>
    public class SixPackServiceLocators
    {
        public static Func<IAsset> IAssetCreationFactory { private get; set; }
        public IAsset GetAssetInstance() 
        {
            return IAssetCreationFactory != null ? IAssetCreationFactory() : new Asset();
        }

        public static Func<IMinifier> IMinifierCreationFactory { private get; set; }
        public IMinifier GetMinifierInstance()
        {
            return IMinifierCreationFactory != null ? IMinifierCreationFactory() : new BundleOnlyMinifier(GetConsumerInstance());
        }

        public static Func<IConsumer> IConsumerCreationFactory { private get; set; }
        public IConsumer GetConsumerInstance()
        {
            return IConsumerCreationFactory != null ? IConsumerCreationFactory() : new Consumer();
        }
    }
}
