using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Creates an intance of IAsset.
        /// </summary>
        public static Func<IAsset> IAssetFactory { protected get; set; }
        
        /// <summary>
        /// Gets and instance of IAsset.
        /// </summary>
        /// <returns></returns>
        public IAsset GetAssetInstance() 
        {
            return IAssetFactory != null ? IAssetFactory() : new Asset();
        }

        /// <summary>
        /// A collection of named IMinifier factories
        /// </summary>
        public static IDictionary<string, Func<IMinifier>> IMinifierFacotries = new Dictionary<string, Func<IMinifier>> { };
        
        /// <summary>
        /// Gets an instance of IMinifier
        /// </summary>
        /// <returns></returns>
        public IMinifier GetMinifierInstance(string name)
        {
            if (IMinifierFacotries.ContainsKey(name))
                return IMinifierFacotries[name]();

            return new BundleOnlyMinifier(GetConsumerInstance());
        }

        /// <summary>
        /// Creates an Instance of IConsumer.  This is only used for the local default for GetMinifierInstance, 
        /// so as long as an IMinifierCreationFactory is defined, this does not need to be
        /// </summary>
        public static Func<IConsumer> IConsumerFactory { protected get; set; }
        
        /// <summary>
        /// Gets an Instance of IConsumer.  This is only used for the local default for GetMinifierInstance
        /// </summary>
        /// <returns></returns>
        public IConsumer GetConsumerInstance()
        {
            return IConsumerFactory != null ? IConsumerFactory() : new Consumer();
        }
    }
}
