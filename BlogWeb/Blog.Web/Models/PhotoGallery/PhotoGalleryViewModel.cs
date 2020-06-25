using System;
using System.Collections.Generic;
using Blog.Domain;
using Blog.Retrievers.Gallery;

namespace Blog.Web.Models.Gallery
{
    public class PhotoGalleryViewModel
    {
        public List<GalleryItem> GalleryItems { get; set; }

        public PhotoGalleryViewModel()
        {
            this.GalleryItems = new List<GalleryItem>();
        }

        public PhotoGalleryViewModel(GalleryPagedItemsList galleryPagedItemsList)
            : this(galleryPagedItemsList.Items)
        {
        }

        public PhotoGalleryViewModel(IEnumerable<GalleryItem> galleryItems)
            : this()
        {
            this.GalleryItems.AddRange(galleryItems);
        }
    }
}
