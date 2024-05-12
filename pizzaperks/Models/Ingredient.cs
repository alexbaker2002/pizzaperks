namespace pizzaperks.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; } = "N/A";
        public string Description { get; set; } = "N/A";
        public double Cost { get; set; } = 0.00;
    }
}
