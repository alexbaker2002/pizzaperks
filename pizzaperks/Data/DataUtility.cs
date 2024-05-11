using Microsoft.AspNetCore.Identity;
using pizzaperks.Models;
using pizzaperks.Models.Enums;

namespace pizzaperks.Data
{
    public class DataUtility
    {
        public DataUtility() { }

        public static async Task ManageDataAsync(IHost host)
        {


            using var svcScope = host.Services.CreateScope();
            var svcProvider = svcScope.ServiceProvider;

            //Service: An instance of DatabaseManager
            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
            //Service: An instance of RoleManager
            var roleManagerSvc = svcProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //Service: An instance of the UserManager
            var userManagerSvc = svcProvider.GetRequiredService<UserManager<PZUser>>();
            //Migration: This is the programmatic equivalent to Update-Database

            await SeedRolesAsync(roleManagerSvc);
            await SeedDemoUsersAsync(userManagerSvc);




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
                UserName = "Dustin Manager",
                Email = "dman@pizzaperks.com",
                FirstName = "Dustin",
                LastName = "Manager",
                EmailConfirmed = true,

            };
            try
            {

                await userManager.CreateAsync(defaultUser, "Abc&123!");
                await userManager.AddToRoleAsync(defaultUser, Roles.Manager.ToString());


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
                UserName = "Christy Customer",
                Email = "christy@notmymail.com",
                FirstName = "Christy",
                LastName = "Customer",
                EmailConfirmed = true,

            };
            try
            {

                await userManager.CreateAsync(defaultCustomer, "Abc&123!");
                await userManager.AddToRoleAsync(defaultCustomer, Roles.Customer.ToString());


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
