using Microsoft.AspNetCore.Identity;

namespace pizzaperks.Models
{
    public class PZUser : IdentityUser
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }






    }
}
