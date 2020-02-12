using Blog.Domain;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers.Administration
{
    [Authorize(Roles = ApplicationRole.Administrator)]
    [Route("Administration/[controller]/[action]")]
    public class BaseAdministrationController : BaseController
    {
        public BaseAdministrationController(ILog log)
            : base(log)
        {
        }
    }
}
