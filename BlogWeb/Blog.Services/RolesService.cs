using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Domain;
using Microsoft.AspNetCore.Identity;

namespace Blog.Services
{
    public interface IRolesService
    {
        Task<string> ValidateRoles(IEnumerable<string> roles);
    }

    public class RolesService : Service, IRolesService
    {
        private readonly RoleManager<ApplicationRole> roleManager;

        public RolesService(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task<string> ValidateRoles(IEnumerable<string> roles)
        {
            foreach (string role in roles)
            {
                bool roleCheck = await this.roleManager.RoleExistsAsync(role);
                if (!roleCheck)
                {
                    string roleDoesNotExistResponseMessageText = String.Concat("The given role ", role, " does not exist in the system");
                    return roleDoesNotExistResponseMessageText;
                }
            }
            return String.Empty;
        }
    }
}
