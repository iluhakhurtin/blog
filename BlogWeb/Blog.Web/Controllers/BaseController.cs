using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ILog log;

        public BaseController(ILog log)
        {
            this.log = log;
        }
    }
}
