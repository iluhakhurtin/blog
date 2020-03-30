using System;
using Blog.Domain;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers.Administration
{
    [Route("Administration/Gallery")]
    public class GalleryController : BaseAdministrationController
    {
        public GalleryController(ILog log)
            : base(log)
        {

        }

        public IActionResult Index()
        {
            return View("~/Views/Administration/Gallery.cshtml");
        }
    }
}
