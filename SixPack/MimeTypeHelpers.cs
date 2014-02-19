using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixPack
{
    public static class MimeTypeHelpers
    {
        const string _defaultMimeType = "application/octet-stream";

        public static Dictionary<string, string> MimeTypes = new Dictionary<string, string> { 
            {"js", "application/javascript"},
            {"css", "text/css"},
            {"less", "text/css"},
            {"sass", "text/css"}
            // TODO: flesh this out more (take a look at Nancy.MimeTypes for a more complete list
        };

        /// <summary>
        /// Get the extension for a given path or fileName
        /// </summary>
        /// <param name="pathOrFileName"></param>
        /// <returns></returns>
        public static string GetExtension(string pathOrFileName) 
        {
            return Path.GetExtension(pathOrFileName);

            //    string result = null;
            //    int dot = fileName.LastIndexOf('.');

            //    if (dot != -1 && fileName.Length > dot + 1)
            //        MimeTypes.TryGetValue(fileName.Substring(dot + 1), out result);
        }
        
        /// <summary>
        /// Get the MimeType that matches a given extension
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GetMimeType(string extension) 
        {
            if (String.IsNullOrWhiteSpace(extension))
                return _defaultMimeType;

            if (extension.Contains('.'))
                extension = GetExtension(extension);

            string _result = null;
            MimeTypes.TryGetValue(extension, out _result);
            return _result != null ? _result : _defaultMimeType;
        }
    }
}
