using System.ComponentModel;

namespace pizzaperks.Models
{
    public class OrderStatus
    {
        public int Id { get; set; }

        [DisplayName("Order Status")]
        public string Name { get; set; }
    }
}
