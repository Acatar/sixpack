using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixPack.Locale
{
    public class Locale_En : ILocale
    {
        private string MissingImplementation = "Are you missing an implementation?";

        public string MinifierNotImplemented
        {
            get
            {
                return "IMinifier cannot be null and must be implemented. " + MissingImplementation;
            }
        }

        public string CacheProviderNotImplemented
        {
            get
            {
                return "ICacheProvider cannot be null and must be implemented. " + MissingImplementation;
            }
        }
    }
}
