using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pizzaperks.Models;
using pizzaperks.Services.Interfaces;
using System.Diagnostics;

namespace pizzaperks.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IOrdersService ordersService) : Controller
    {


        private readonly ILogger<HomeController>? _logger = logger;
        private readonly IOrdersService _ordersService = ordersService;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Dashboard()
        {


            //TODO: Create IOrdersService
            //Get all Orders that do not show complete and send to Dashboard View

            List<Order> orders = await _ordersService.GetAllOrdersAsync();


            return View(orders);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
