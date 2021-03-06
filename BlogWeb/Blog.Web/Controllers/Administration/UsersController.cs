﻿using System;
using Blog.Domain;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers.Administration
{
    public class UsersController : BaseAdministrationController
    {
        public UsersController(ILog log)
            : base(log)
        {

        }

        public IActionResult Index()
        {
            return View("~/Views/Administration/Users.cshtml");
        }
    }
}
