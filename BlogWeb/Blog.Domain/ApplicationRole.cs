using System;
using Microsoft.AspNetCore.Identity;

namespace Blog.Domain
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public const string Administrator = "Administrator";
        public const string PrivateReader = "PrivateReader";
        public const string ImageViewer = "ImageViewer";


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
