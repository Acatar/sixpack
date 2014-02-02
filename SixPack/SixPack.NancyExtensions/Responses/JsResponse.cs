using System.Collections.Generic;
using System.Text;
using Nancy;
using Nancy.Cookies;
using Nancy.Responses;

namespace SixPack.NancyExtensions.Responses
{
    /// <summary>
    /// Represents a javascript ("application/javascript") response
    /// </summary>
    public class JsResponse : TextResponse
    {
        const string jsMimeType = "application/javascript";

        /// <summary>
        /// Creates a new instance of the JsResponse class
        /// </summary>
        /// <param name="contents">Text content - defaults to empty if null</param>
        /// <param name="contentType">Content Type - defaults to application/javascript</param>
        /// <param name="encoding">String encoding - UTF8 if null</param>
        public JsResponse(string contents, string contentType = jsMimeType, Encoding encoding = null)
            : base(contents, contentType: contentType, encoding: encoding)
        {

        }

        /// <summary>
        /// Creates a new instance of the JsResponse class
        /// </summary>
        /// <param name="statusCode">Status code - defaults to OK</param>
        /// <param name="contents">Text content - defaults to empty if null</param>
        /// <param name="encoding">String encoding - UTF8 if null</param>
        /// <param name="headers">Headers if required</param>
        /// <param name="cookies">Cookies if required</param>
        public JsResponse(HttpStatusCode statusCode = HttpStatusCode.OK, string contents = null, Encoding encoding = null, IDictionary<string, string> headers = null, IEnumerable<INancyCookie> cookies = null)
            : base(statusCode: statusCode, contents: contents, encoding: encoding, headers: headers, cookies: cookies)
        {

        }
    }
}