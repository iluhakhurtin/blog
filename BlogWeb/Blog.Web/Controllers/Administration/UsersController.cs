using System;
using Blog.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers.Administration
{
    [Authorize(Roles = ApplicationRole.Administrator)]
    [Route("Administration/[controller]/[action]")]
    public class UsersController : BaseController
    {
        public IActionResult Index()
        {
            return View("~/Views/Administration/Users.cshtml");
        }
    }
}
