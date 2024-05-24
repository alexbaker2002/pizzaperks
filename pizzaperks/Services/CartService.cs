using Microsoft.EntityFrameworkCore;
using pizzaperks.Data;
using pizzaperks.Models;
using pizzaperks.Services.Interfaces;

namespace pizzaperks.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartService> _logger;



        public CartService(ApplicationDbContext context, ILogger<CartService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Cart> CreateNewCartAsync(PZUser user)
        {
            if (user is not null)
                try
                {

                    Cart newCart = new Cart();
                    // add user to cart
                    newCart.User = user;
                    newCart.PzUserId = user.Id;

                    //add cart to db
                    _context.Add(newCart);
                    // add user
                    user.CartId = newCart.Id;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return newCart;
                }
                catch (Exception ex)
                {
                    _logger.LogError(80, ex.Message);
                    return null!;
                }
            return null!;
        }

        public async Task<Cart> GetCartWithItemsAsync(PZUser user)
        {
            //TODO: Left off Here!!
            if (user is null)
            {
                return new Cart();
            }



            Cart? cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == user.CartId);
            if (cart is null)
            {
                return new Cart();
            }


            return cart;

        }


    }
}
