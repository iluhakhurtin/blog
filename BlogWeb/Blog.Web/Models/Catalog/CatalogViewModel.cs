using System;
using System.Collections.Generic;
using Blog.Domain;

namespace Blog.Web.Models.Catalog
{
    public class CatalogViewModel
    {
        public IList<Category> Categories { get; set; }

        public CatalogViewModel()
        {

        }

        public CatalogViewModel(IList<Category> categories)
        {
            this.Categories = categories;
        }
    }
}
