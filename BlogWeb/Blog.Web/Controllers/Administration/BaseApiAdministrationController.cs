using Blog.Domain;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers.Administration
{
    [Authorize(Roles = ApplicationRole.Administrator)]
    [Produces("application/json")]
    public class BaseApiAdministrationController : BaseApiController
    {
        public BaseApiAdministrationController(ILog log)
            : base(log)
        {
        }
    }
}
