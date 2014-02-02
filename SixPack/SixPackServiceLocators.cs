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
        /// A collection of named IMinifier factories
        /// </summary>
        public static IDictionary<string, Func<IMinifier>> IMinifierFactories = new Dictionary<string, Func<IMinifier>> { };
        
        /// <summary>
        /// Gets an instance of IMinifier.  If a minifier with the given name is not found, a local default is returned instead.
        /// </summary>
        /// <returns></returns>
        public IMinifier GetMinifierInstance(string name)
        {
            if(String.IsNullOrWhiteSpace(name))
                return new BundleOnlyMinifier(new Consumer());

            if (IMinifierFactories.ContainsKey(name))
                return IMinifierFactories[name]();

            return new BundleOnlyMinifier(new Consumer());
        }

        /// <summary>
        /// Check to see if a factory with a given name exists in the IMinifierFactories dictionary
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool MinifierFactoryExists(string name) 
        {
            return IMinifierFactories.ContainsKey(name);
        }
    }
}
