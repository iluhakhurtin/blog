using System;
namespace Blog.Retrievers.Article
{
    public class ArticleWithRolesDataResult
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public String Body { get; set; }
        public String Roles { get; set; }
        public DateTime Timestamp { get; set; }

        public string[] GetRoles()
        {
            if (String.IsNullOrEmpty(this.Roles))
                return null;
            return this.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
