using pizzaperks.Models;


namespace pizzaperks.Services.Interfaces
{
    public interface ICartService
    {
        Task AddToCartAsync(Product product, PZUser user);
        Task<Cart> CreateNewCartAsync(PZUser user);
        Task<Cart> GetCartWithItemsAsync(PZUser user);


    }
}
