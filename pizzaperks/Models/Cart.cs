using System.ComponentModel.DataAnnotations.Schema;

namespace pizzaperks.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string? PzUserId { get; set; }

        [NotMapped]
        public virtual PZUser? User { get; set; }
        public virtual ICollection<CartProduct> Products { get; set; } = new HashSet<CartProduct>();
    }
}
