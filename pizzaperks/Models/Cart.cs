using System.ComponentModel.DataAnnotations.Schema;

namespace pizzaperks.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string? PzUserId { get; set; }

        [NotMapped]
        public virtual PZUser? User { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
