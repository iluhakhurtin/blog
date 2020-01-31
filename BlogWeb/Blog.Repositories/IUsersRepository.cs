using System;
namespace Blog.Repositories
{
    public interface IUsersRepository
    {
        Task<UsersPagedDataTable> GetUsersPagedAsync(
            string emailFiler,
            string firstNameFilter,
            string lastNameFilter,
            string genderFilter,
            string phoneNumberFilter,
            string clientDbNameFilter,
            string clientDbServerFilter,
            string kitGoogleAccountFilter,
            string sortColumn,
            string sortOrder,
            int pageNumber,
            int pageSize
        );
    }
}
