namespace pizzaperks.Models
{
    public class CartProduct
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Cost { get; set; }

        public virtual ICollection<OrderedIngredient> Ingredients { get; set; } = new HashSet<OrderedIngredient>();

        public virtual ICollection<OrderModification> Modifications { get; set; } = new HashSet<OrderModification>();
    }
}
