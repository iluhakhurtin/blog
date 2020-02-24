using System;
using Blog.Domain;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers.Administration
{
    [Route("Administration/Files")]
    public class FilesController : BaseAdministrationController
    {
        public FilesController(ILog log)
            : base(log)
        {

        }

        public IActionResult Index()
        {
            return View("~/Views/Administration/Files.cshtml");
        }
    }
}
