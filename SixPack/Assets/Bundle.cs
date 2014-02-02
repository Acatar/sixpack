using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixPack.Assets
{
    public class Bundle
    {
        /// <summary>
        /// The name of the bundle
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// An array of files to be bundled
        /// </summary>
        public ICollection<string> FilePathArray { get; set; }

        /// <summary>
        /// The bundled content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The extenion that the content has (i.e. js, css, less)
        /// </summary>
        public string ContentExtension { get; set; }

        /// <summary>
        /// The mime type of the bundle
        /// </summary>
        public string MimeType 
        { 
            get 
            {
                if (!String.IsNullOrWhiteSpace(_mimeType))
                    return _mimeType;
                if (!String.IsNullOrWhiteSpace(ContentExtension))
                    return SixPackHelpers.MimeTypes.ContainsKey(ContentExtension) ? SixPackHelpers.MimeTypes[ContentExtension] : null;

                return null;
            }
            set 
            {
                _mimeType = value;
            }
        }
        private string _mimeType { get; set; }
    }
}
