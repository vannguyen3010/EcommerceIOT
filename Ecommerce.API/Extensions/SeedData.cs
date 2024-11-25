using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.API.Extensions
{
    public class SeedData
    {
        public static async Task SeedUsers(UserManager<User> userManager)
        {
            if (await userManager.FindByNameAsync("admin001") == null)
            {
                var adminUser = new User
                {
                    UserName = "admin001",
                    Email = "admin001@matech.com",
                };

                await userManager.CreateAsync(adminUser, "AdminPassword123");
            }

            if (await userManager.FindByNameAsync("user002") == null)
            {
                var normalUser = new User
                {
                    UserName = "user002",
                    Email = "user002@matech.com",
                };

                await userManager.CreateAsync(normalUser, "UserPassword123");
            }
        }
    }
}
