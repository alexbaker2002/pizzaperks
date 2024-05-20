using System.ComponentModel;

namespace pizzaperks.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? OrderNumber { get; set; }
        public double OrderTotal { get; set; }
        public string? CustomerName { get; set; }
        public string? OrderStatus { get; set; }

        [DisplayName("Order Date/Time")]
        public DateTime OrderDateTime { get; set; }
        public string? PZUserId { get; set; }

        //Nav
        public virtual PZUser? CustomerAccount { get; set; }
        public virtual ICollection<Product> OrderedItems { get; set; } = new HashSet<Product>();


    }
}
