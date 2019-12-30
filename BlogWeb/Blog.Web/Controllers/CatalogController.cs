using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Services;
using Blog.Web.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.Web.Controllers
{
    public class CatalogController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public CatalogController(IServices services)
        {
            this.categoriesService = services.CategoriesService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var categories = await this.categoriesService.GetAllCategoriesTree();
            var catalogViewModel = new CatalogViewModel(categories);

            return View(catalogViewModel);
        }

        //public async Task<IActionResult> SideCategory()
        //{

        //}
    }
}
