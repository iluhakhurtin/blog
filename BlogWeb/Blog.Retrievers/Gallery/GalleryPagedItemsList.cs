using System;
using Blog.Domain;

namespace Blog.Retrievers.Gallery
{
    public class GalleryPagedItemsList : PagedItemsList<GalleryItem>
    {
        public GalleryPagedItemsList(int pageNumber, int pageSize)
            : base(pageNumber, pageSize)
        {
        }
    }
}
