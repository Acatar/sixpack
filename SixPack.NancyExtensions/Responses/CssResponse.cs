using System.Collections.Generic;
using System.Text;
using Nancy;
using Nancy.Cookies;
using Nancy.Responses;

namespace SixPack.NancyExtensions.Responses
{
    /// <summary>
    /// Represents a javascript (text/css) response
    /// </summary>
    public class CssResponse : TextResponse
    {
        const string cssMimeType = "text/css";

        /// <summary>
        /// Creates a new instance of the CssResponse class
        /// </summary>
        /// <param name="contents">Text content - defaults to empty if null</param>
        /// <param name="contentType">Content Type - defaults to text/css</param>
        /// <param name="encoding">String encoding - UTF8 if null</param>
        public CssResponse(string contents, string contentType = cssMimeType, Encoding encoding = null)
            : base(contents, contentType: contentType, encoding: encoding) 
        { 
        
        }

        /// <summary>
        /// Creates a new instance of the CssResponse class
        /// </summary>
        /// <param name="statusCode">Status code - defaults to OK</param>
        /// <param name="contents">Text content - defaults to empty if null</param>
        /// <param name="encoding">String encoding - UTF8 if null</param>
        /// <param name="headers">Headers if required</param>
        /// <param name="cookies">Cookies if required</param>
        public CssResponse(HttpStatusCode statusCode = HttpStatusCode.OK, string contents = null, Encoding encoding = null, IDictionary<string, string> headers = null, IEnumerable<INancyCookie> cookies = null)
            : base(statusCode: statusCode, contents: contents, encoding: encoding, headers: headers, cookies: cookies) 
        { 
        
        }
    }
}