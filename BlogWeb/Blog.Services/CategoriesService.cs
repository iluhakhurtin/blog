using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Repositories;
using System.Linq;

namespace Blog.Services
{
    public interface ICategoriesService
    {
        Task<IList<Category>> GetAllCategoriesTree();
    }

    public class CategoriesService : Service, ICategoriesService
    {
        private readonly ICategoriesRepository categoriesRepository;

        public CategoriesService(IRepositories repositories)
        {
            this.categoriesRepository = repositories.CategoriesRepository;
        }

        public async Task<IList<Category>> GetAllCategoriesTree()
        {
            var allCategories = await this.categoriesRepository.GetAllAsync();
            foreach (var category in allCategories)
            {
                category.Children = GetCategoryChildren(allCategories, category);
            }
            return allCategories.Where(c => c.ParentId == null).ToList();
        }

        private IList<Category> GetCategoryChildren(IList<Category> allCategories, Category category)
        {
            //base case
            if (allCategories.All(c => c.ParentId != category.Id))
                return null;

            //recursive case
            category.Children = allCategories
                .Where(c => c.ParentId == category.Id)
                .ToList();

            foreach (var item in category.Children)
            {
                item.Children = GetCategoryChildren(allCategories, item);
            }

            return category.Children;
        }
    }
}
