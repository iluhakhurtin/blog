using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.Web.Controllers
{
    public class ImageController : Controller
    {
        public IActionResult Index(Guid id)
        {
            //"c8c9a1b1-5505-231d-93cc-cec4e8d03820"

            return View();
        }
    }
}
