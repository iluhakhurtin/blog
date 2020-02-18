using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Repositories;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers.Administration
{
    [Route("api/Administration/CategoriesApi")]
    public class CategoriesApiController : BaseApiAdministrationController
    {
        private readonly ICategoriesRepository categoriesRepository;

        public CategoriesApiController(IRepositories repositories, ILog log)
            : base(log)
        {
            this.categoriesRepository = repositories.CategoriesRepository;
        }

        // GET: api/CategoriesApi/GetAll
        [HttpGet("GetAll")]
        public async Task<IEnumerable<KeyValuePair<Guid, String>>> GetAll()
        {
            try
            {
                var categories = await this.categoriesRepository.GetAllAsync();
                var result = new List<KeyValuePair<Guid, String>>();
                foreach(var category in categories)
                {
                    var categoryResult = new KeyValuePair<Guid, String>(category.Id, category.Name);
                    result.Add(categoryResult);
                }
                return result;
            }
            catch (Exception ex)
            {
                if (base.log.IsErrorEnabled)
                    base.log.Error(ex);
            }
            return null;
        }
    }
}
