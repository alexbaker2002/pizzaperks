namespace pizzaperks.Models
{
    public class PurchasedProduct
    {
        public int Id { get; set; }
        public string? OrderNumber { get; set; }
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }



    }
}
