using System;
namespace Blog.Services
{
    public interface IServices
    {
        IImagesService ImagesService { get; }
    }
    
    public class Services
    {
        public Services()
        {
        }
    }
}
