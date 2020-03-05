using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Blog.Web.Models;
using log4net;
using Blog.Retrievers;
using Blog.Retrievers.Article;
using Blog.Web.Models.Home;

namespace Blog.Web.Controllers
{
    public class AboutController : BaseController
    {
        public AboutController(ILog log)
            : base(log)
        {
        }

        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch(Exception ex)
            {
                if(base.log.IsErrorEnabled)
                    base.log.Error(ex);

                return base.NotFound();
            }
        }
    }
}
