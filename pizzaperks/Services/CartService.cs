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

        public async Task AddIngredienttoCartProductAsync(OrderedIngredient ingredient, CartProduct product)
        {
            _context.OrderedIngredients.Add(ingredient);
            product.Ingredients.Add(ingredient);
            _context.CartProducts.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<CartProduct?> AddToCartAsync(CartProduct product, PZUser user)
        {
            if (user is null)
            {
                return null;
            }
            try
            {
                foreach (Ingredient item in product.Ingredients)
                {
                    item.Cost = 0;
                }

                Cart? cart = await _context.Carts.FirstOrDefaultAsync(c => c.PzUserId == user.Id);
                if (cart is not null)
                {
                    cart.Products.Add(product);
                    _context.Carts.Update(cart);
                }

                await _context.SaveChangesAsync();
                return product;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }


        }

        public async Task<double> CalculateCartTotalAsync(Cart cart)
        {
            double total = 0;

            await Task.Run(() =>
            {
                foreach (CartProduct product in cart.Products)
                {
                    total += product.Cost;
                }
            });

            return total;

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

        public async Task DeleteFromCartProductAsync(CartProduct product)
        {

            if (product == null)
            {
                return;
            }
            _context.CartProducts.Remove(product);
            await _context.SaveChangesAsync();

        }

        public async Task<CartProduct?> GetCartProductAsync(int? itemId)
        {
            try
            {
                return await _context.CartProducts
                    .Include(c => c.Ingredients)
                    .Include(c => c.Modifications)
                    .FirstOrDefaultAsync(c => c.Id == itemId);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Cart> GetCartWithItemsAsync(PZUser user)
        {

            if (user is null)
            {
                return new Cart();
            }



            Cart? cart = await _context.Carts.Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.PzUserId == user.Id);
            if (cart is null)
            {
                return new Cart();
            }


            return cart;

        }

        public async Task RemoveIngredientFromCartProductAsync(OrderedIngredient ingredient)
        {
            _context.OrderedIngredients.Remove(ingredient);
            await _context.SaveChangesAsync();

        }

        public async Task<CartProduct?> UpdateCartProductAsync(CartProduct product)
        {
            _context.CartProducts.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
