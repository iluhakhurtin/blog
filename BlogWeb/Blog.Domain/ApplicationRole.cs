using System;
using Microsoft.AspNetCore.Identity;

namespace Blog.Domain
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
            :base()
        {

        }

        public ApplicationRole(string roleName)
            : base(roleName)
        {

        }
    }
}
