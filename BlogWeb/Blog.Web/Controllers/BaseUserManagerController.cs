using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Domain;
using log4net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public abstract class BaseUserManagerController : BaseController
    {
        protected readonly UserManager<ApplicationUser> userManager;

        public BaseUserManagerController(ILog log, UserManager<ApplicationUser> userManager)
            : base(log)
        {
            this.userManager = userManager;
        }

        protected async Task<IList<string>> GetUserRoles()
        {
            IList<string> userRoles = new List<string>();
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var user = await this.userManager.GetUserAsync(HttpContext.User);
                userRoles = await this.userManager.GetRolesAsync(user);
            }

            return userRoles;
        }
    }
}
