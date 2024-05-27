using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pizzaperks.Data;
using pizzaperks.Models;
using pizzaperks.Models.ViewModels;
using pizzaperks.Services.Interfaces;

namespace pizzaperks.Controllers
{
    [Authorize]
    public class CartController(ApplicationDbContext context, UserManager<PZUser> userManager,
        ICartService cartService,
        IDataService dataService) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<PZUser> _userManager = userManager ?? null!;
        private readonly ICartService _cartService = cartService ?? null!;
        private readonly IDataService _dataService = dataService ?? null!;

        // GET: User Cart with Items

        public async Task<IActionResult> Index()
        {

            if (_userManager == null)
            {
                return NotFound();
            }
            PZUser? user = await _userManager.GetUserAsync(User);

            if (user is null)
            {
                return View(null);
            }
            int cartId = user!.CartId;


            Cart cart = await _cartService.GetCartWithItemsAsync(user);



            CartIndexViewModel model = new();
            model.OrderTotal = Math.Round(await _cartService.CalculateCartTotalAsync(cart), 2);

            model.DefaultProducts = await _dataService.GetProductsAsync();
            model.DefaultIngredients = await _dataService.GetIngredientsAsync();
            model.CartProducts = cart.Products;


            return View(model);
        }

        [HttpGet]

        public async Task<IActionResult> AddItem(int? Id)
        {
            PZUser? user = await _userManager.GetUserAsync(User);
            Product? item = await _dataService.GetProductAsync(Id);

            CartProduct cartProduct = new CartProduct
            {
                Name = item!.Name,
                Description = item!.Description,
                Cost = item!.Cost
            };

            foreach (ProductIngredient ingredient in item.Ingredients)
            {
                cartProduct.Ingredients.Add(new OrderedIngredient
                {
                    Name = ingredient.Name,
                    Description = ingredient.Description,
                    Cost = ingredient.Cost,
                    Double = false,
                    Remove = false

                });
            }



            if (item is null || user is null)
            {
                return NotFound();
            }

            await _cartService.AddToCartAsync(cartProduct, user);

            if (item is null)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> EditCartItem(int? Id)
        {

            CartProduct? product = await _cartService.GetCartProductAsync(Id);


            EditCartProductViewModel model = new();
            model.CartProduct = product;
            model.DefaultIngredients = await _dataService.GetIngredientsAsync();


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddIngredient(int Id, int productId)
        {
            CartProduct? cartProduct = await _cartService.GetCartProductAsync(productId);
            Ingredient? ingredient = await _dataService.GetIngredientByIdAsync(Id);
            OrderedIngredient orderedIngredient = new OrderedIngredient()
            {
                Name = ingredient!.Name,
                Description = ingredient.Description,
                Double = false,
                Cost = ingredient.Cost,
                Remove = false
            };

            await _cartService.AddIngredienttoCartProductAsync(orderedIngredient, cartProduct!);
            cartProduct!.Cost += ingredient!.Cost;
            cartProduct.Cost = Math.Round(cartProduct.Cost, 2);

            await _cartService.UpdateCartProductAsync(cartProduct);

            return RedirectToAction("EditCartItem", "Cart", new { Id = productId });
        }

        [HttpGet]
        public async Task<IActionResult> RemoveIngredient(int? Id, int? productId)
        {
            CartProduct? product = await _cartService.GetCartProductAsync(productId);
            OrderedIngredient? ingredient = product!.Ingredients.FirstOrDefault(c => c.Id == Id);

            await _cartService.RemoveIngredientFromCartProductAsync(ingredient!);

            product.Cost -= ingredient!.Cost;
            product.Cost = Math.Round(product.Cost, 2);

            await _cartService.UpdateCartProductAsync(product);




            return RedirectToAction("EditCartItem", "Cart", new { Id = productId });
        }





        [HttpGet]
        public async Task<IActionResult> RemoveProductFromCart(int? Id)
        {

            CartProduct? product = await _cartService.GetCartProductAsync(Id);
            if (product is null)
            {
                return RedirectToAction("Index");
            }
            await _cartService.DeleteFromCartProductAsync(product);
            return RedirectToAction("Index");
        }


    }
}
