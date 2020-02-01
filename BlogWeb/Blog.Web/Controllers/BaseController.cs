using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public IActionResult AccessDenied()
        {
            return base.RedirectToAction("AccessDenied", "Account");
        }
    }
}
