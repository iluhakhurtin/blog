using System;
using System.Collections.Generic;
using Blog.Domain;
using Blog.Retrievers.Gallery;

namespace Blog.Web.Models.Gallery
{
    public class PhotoGalleryPreviewViewModel
    {
        public Guid ImageId { get; set; }
        public string PageTitle { get; set; }
        public string PhotoTitle { get; set; }
        public string PhotoDescription { get; set; }

        public PhotoGalleryPreviewViewModel()
        {

        }

        public PhotoGalleryPreviewViewModel(GalleryItem galleryItem)
            : this()
        {
            this.ImageId = galleryItem.ImageId;
            this.PhotoTitle = galleryItem.Title;
            this.PageTitle = this.CutString(galleryItem.Title, 100);
            this.PhotoDescription = galleryItem.Description;
        }

        private string CutString(string input, int count)
        {
            if (String.IsNullOrEmpty(input))
                return input;

            if (input.Length <= count)
                return input;

            return input.Substring(0, count - 3) + "...";
        }
    }
}
