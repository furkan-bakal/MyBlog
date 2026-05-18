using Microsoft.AspNetCore.Identity;
using Repository.Identity;
using System.Security.Claims;

namespace Service.User
{
    public class UserSeedData
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("admin");
            if (adminRole is null)
            {
                await roleManager.CreateAsync(new AppRole { Name = "admin" });
                adminRole = await roleManager.FindByNameAsync("admin");
            }

            var adminRoleClaims = await roleManager.GetClaimsAsync(adminRole!);

            if (!adminRoleClaims.Any())
            {

                await roleManager.AddClaimAsync(adminRole!, new Claim("create", "true"));
                await roleManager.AddClaimAsync(adminRole!, new Claim("update", "true"));
                await roleManager.AddClaimAsync(adminRole!, new Claim("delete", "true"));
            }

            var user = userManager.Users.FirstOrDefault();

            if (user is null)
            {
                user = new AppUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "admin",
                    Email = "admin@example.com",
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                
                await userManager.CreateAsync(user, "Password12*");
            }

            if (!await userManager.IsInRoleAsync(user!, "admin"))
            {
                await userManager.AddToRoleAsync(user!, "admin");
            }
        }
    }
}
