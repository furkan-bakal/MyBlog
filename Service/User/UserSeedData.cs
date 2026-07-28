using Microsoft.AspNetCore.Identity;
using Repository.Identity;
using System.Security.Claims;

namespace Service.User
{
    public class UserSeedData
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SeedUserOptions seedUser)
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
                if (string.IsNullOrWhiteSpace(seedUser.UserName) || string.IsNullOrWhiteSpace(seedUser.Password))
                {
                    throw new InvalidOperationException(
                        "Admin kullanıcısı yok ve seed bilgileri tanımlı değil. " +
                        "SeedUser__UserName / SeedUser__Password (opsiyonel: SeedUser__Email) ortam değişkenlerini " +
                        "veya user-secrets değerlerini ayarlayın.");
                }

                user = new AppUser
                {
                    Id = Guid.NewGuid(),
                    UserName = seedUser.UserName,
                    Email = string.IsNullOrWhiteSpace(seedUser.Email) ? null : seedUser.Email,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await userManager.CreateAsync(user, seedUser.Password);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        "Seed admin kullanıcısı oluşturulamadı: " +
                        string.Join(" | ", result.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(user, "admin"))
            {
                await userManager.AddToRoleAsync(user, "admin");
            }
        }
    }
}
