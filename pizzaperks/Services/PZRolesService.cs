using Microsoft.AspNetCore.Identity;
using pizzaperks.Data;
using pizzaperks.Models;
using pizzaperks.Services.Interfaces;

namespace pizzaperks.Services
{
    public class PZRolesService(RoleManager<IdentityRole> roleManager, UserManager<PZUser> userManager, ApplicationDbContext context) : IPZRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly UserManager<PZUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;

        public async Task<bool> AddUserToRoleAsync(PZUser user, string roleName)
        {
            try
            {// wraps the AddToRoleAsync so we can use .Succeeded to return the boolean
                bool result = (await _userManager.AddToRoleAsync(user, roleName)).Succeeded;
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
                throw;
            }
        }
    }
}
