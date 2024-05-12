using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace pizzaperks.Models
{
    public class PZUser : IdentityUser
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }


        [NotMapped]
        public string FullName { get { return $"{FirstName} {LastName}"; } }





    }
}
