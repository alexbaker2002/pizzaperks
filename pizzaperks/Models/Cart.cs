namespace pizzaperks.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
