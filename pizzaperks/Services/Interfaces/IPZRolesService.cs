using pizzaperks.Models;

namespace pizzaperks.Services.Interfaces
{
    public interface IPZRolesService
    {
        Task<bool> AddUserToRoleAsync(PZUser user, string roleName);
    }
}
