using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SixPack.HttpHandlers
{
    public class DefaultHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            

            //// Write the response out to the client
            //context.Response.ContentType = "text/css";
            //context.Response.Write(_css);
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
