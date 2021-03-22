using System;
using System.Collections.Generic;
using Blog.Domain;
using Blog.Retrievers.Gallery;

namespace Blog.Web.Models.Gallery
{
    public class PhotoGalleryViewModel : IPagerViewModel
    {
        #region IPaginationViewModel Props

        public int TotalPagesCount { get; set; }

        public int PageNumber { get; set; }

        #endregion

        public List<GalleryItem> GalleryItems { get; set; }

        public PhotoGalleryViewModel()
        {
            this.GalleryItems = new List<GalleryItem>();
        }

        public PhotoGalleryViewModel(GalleryPagedItemsList galleryPagedItemsList)
            : this(galleryPagedItemsList.Items)
        {
            this.TotalPagesCount = galleryPagedItemsList.TotalPagesCount;
            this.PageNumber = galleryPagedItemsList.PageNumber;
        }

        public PhotoGalleryViewModel(IEnumerable<GalleryItem> galleryItems)
            : this()
        {
            this.GalleryItems.AddRange(galleryItems);
        }
    }
}
