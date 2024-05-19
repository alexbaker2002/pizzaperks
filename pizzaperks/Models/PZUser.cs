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
        public int CartId { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();




    }
}
