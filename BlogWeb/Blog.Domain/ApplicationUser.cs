using System;
using Microsoft.AspNetCore.Identity;

namespace Blog.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
            : base()
        {
        }

        public ApplicationUser(string userName)
            : base(userName)
        {

        }
    }
}
