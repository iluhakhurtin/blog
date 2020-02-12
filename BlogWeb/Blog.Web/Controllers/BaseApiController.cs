using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using log4net;

namespace Blog.Web.Controllers
{
    public abstract class BaseApiController : BaseController
    {
        public BaseApiController(ILog log)
            : base(log)
        {

        }

        protected HttpResponseMessage CreateErrorResponseMessage(string message = null)
        {
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            if (!String.IsNullOrEmpty(message))
                httpResponseMessage.ReasonPhrase = this.FormatMessage(message);
            return httpResponseMessage;
        }

        protected HttpResponseMessage CreateOkResponseMessage(string message = null)
        {
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            if (!String.IsNullOrEmpty(message))
                httpResponseMessage.ReasonPhrase = this.FormatMessage(message);
            return httpResponseMessage;
        }

        private string FormatMessage(string message)
        {
            if (String.IsNullOrEmpty(message))
                return message;
            string withoutNewLineChars = Regex.Replace(message, @"\t|\n|\r", "");
            return withoutNewLineChars;
        }
    }
}
