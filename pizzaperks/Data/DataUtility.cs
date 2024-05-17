using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pizzaperks.Models;
using pizzaperks.Models.Enums;

namespace pizzaperks.Data
{
    public class DataUtility()
    {



        public static async Task ManageDataAsync(WebApplication app)
        {



            using var svcScope = app.Services.CreateScope();
            var svcProvider = svcScope.ServiceProvider;

            //Service: An instance of DatabaseManager
            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
            //Service: An instance of RoleManager
            var roleManagerSvc = svcProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //Service: An instance of the UserManager
            var userManagerSvc = svcProvider.GetRequiredService<UserManager<PZUser>>();
            //Migration: This is the programmatic equivalent to Update-Database



            await dbContextSvc.Database.MigrateAsync();

            await SeedRolesAsync(roleManagerSvc);

            await SeedDemoUsersAsync(userManagerSvc);

            //TODO: Seed Cart
            // Assign Cart
            //seed ingredients 
            //seed Prodcuts
            // seed orders




        }

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.Manager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Customer.ToString()));
        }

        public static async Task SeedDemoUsersAsync(UserManager<PZUser> userManager)
        {
            //Seed Demo Manager User
            var defaultUser = new PZUser
            {
                UserName = "dman@pizzaperks.com",
                Email = "dman@pizzaperks.com",
                FirstName = "Dustin",
                LastName = "Manager",
                EmailConfirmed = true,
                CartId = -1

            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {

                    var testUser = await userManager.CreateAsync(defaultUser, "Fw%@P!ZZ@8");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Manager.ToString());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Demo Manager User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
            //Seed Demo Manager User
            var defaultCustomer = new PZUser
            {
                UserName = "christy@notmymail.com",
                Email = "christy@notmymail.com",
                FirstName = "Christy",
                LastName = "Customer",
                EmailConfirmed = true,
                CartId = 1

            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultCustomer.Email);
                if (user == null)
                {
                    var testUser = await userManager.CreateAsync(defaultCustomer, "Fw%@P!ZZ@8");
                    await userManager.AddToRoleAsync(defaultCustomer, Roles.Customer.ToString());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Demo Customer User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

        }
    }
}
