using System;
using System.Threading.Tasks;

namespace Blog.Retrievers.User
{
    public interface IUsersRetriever : IRetriever
    {
        Task<UsersPagedDataTable> GetUsersPagedAsync(
            string emailFiler,
            string sortColumn,
            string sortOrder,
            int pageNumber,
            int pageSize
        );
    }
}
