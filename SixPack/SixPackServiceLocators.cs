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
        public static IDictionary<string, Func<IMinifier>> IMinifierFacotries = new Dictionary<string, Func<IMinifier>> { };
        
        /// <summary>
        /// Gets an instance of IMinifier
        /// </summary>
        /// <returns></returns>
        public IMinifier GetMinifierInstance(string name)
        {
            if (IMinifierFacotries.ContainsKey(name))
                return IMinifierFacotries[name]();

            return new BundleOnlyMinifier(new Consumer());
        }
    }
}
