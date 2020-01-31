using System;
using System.Data;

namespace Blog.Retrievers.User
{
    public class UsersPagedDataTable : PagedDataTable
    {
        public const string Email = "Email";
        public const string Roles = "Roles";

        public UsersPagedDataTable(int pageNumber, int pageSize)
            : base(pageNumber, pageSize)
        {
            this.InitializeColumns();
        }

        private void InitializeColumns()
        {
            DataColumn column = new DataColumn(Id, typeof(Guid));
            base.Columns.Add(column);

            column = new DataColumn(Email, typeof(string));
            base.Columns.Add(column);

            column = new DataColumn(Roles, typeof(string));
            base.Columns.Add(column);
        }
    }
}
