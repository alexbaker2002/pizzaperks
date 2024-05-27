namespace pizzaperks.Models
{
    public class PurchasedProduct : CartProduct
    {
        public int OrderNumberId { get; set; }

        public virtual Order? Order { get; set; }


    }
}
