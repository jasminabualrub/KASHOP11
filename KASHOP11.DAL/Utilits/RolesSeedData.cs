using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace KASHOP11.DAL.Utilits
{
    public class RolesSeedData : ISeedData
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesSeedData(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task DataSeed()
        {
            string[] roles = { "User", "Admin", "SuperAdmin" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}