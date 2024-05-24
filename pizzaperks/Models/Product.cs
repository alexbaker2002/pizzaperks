namespace pizzaperks.Models
{
    public class Product
    {


        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Cost { get; set; }



        // Ingredients

        public ICollection<Ingredient> Ingredients { get; set; } = new HashSet<Ingredient>();

    }
}
