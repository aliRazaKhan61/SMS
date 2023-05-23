using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;

namespace SMS
{
    public static class MyIdentityDataInitializer
    {
        public static void SeedData
    (UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }
        public static void SeedUsers
    (UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByNameAsync
("admin@gmail.com").Result == null)
            {
                IdentityUser user = new IdentityUser();
                user.UserName = "admin@gmail.com";
                user.Email = "admin@gmail.com";

                IdentityResult result = userManager.CreateAsync
                (user, "1qaz@WSX").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,
                                        "Admin").Wait();
                }
            }


            if (userManager.FindByNameAsync
        ("user@gmail.com").Result == null)
            {
                IdentityUser user = new IdentityUser();
                user.UserName = "user@gmail.com";
                user.Email = "user@gmail.com";

                IdentityResult result = userManager.CreateAsync
                (user, "1qaz@WSX").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,
                                        "User").Wait();
                }
            }
        }
        public static void SeedRoles
    (RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync
("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync
        ("User").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
    }
}
