using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blog.Retrievers;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.Web.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImagesRetriever imagesRetriever;

        public ImageController(IRetrievers retrievers)
        {
            this.imagesRetriever = retrievers.ImagesRetriever;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid id)
        {
            try
            {
                id = Guid.Parse("c8c9a1b1-5505-231d-93cc-cec4e8d03820");
                var imageData = await this.imagesRetriever.GetOriginalImageDataAsync(id);
                FileStreamResult fileStreamResult = new FileStreamResult(imageData.Stream, imageData.MimeType);
            }
            catch
            {
                base.NotFound();
            }
        }
    }
}
