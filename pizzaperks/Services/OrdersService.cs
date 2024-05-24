using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pizzaperks.Data;
using pizzaperks.Models;
using pizzaperks.Models.Enums;
using pizzaperks.Services.Interfaces;

namespace pizzaperks.Services
{
    public class OrdersService(ApplicationDbContext context, UserManager<PZUser> userManager) : IOrdersService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<PZUser> _userManager = userManager;

        public double CalculateOrderTotal(Order order)
        {
            if (order.Alterations)
            {
                //TODO: Modify for Price CalculationS
            }

            double total = order.OrderedItems.Sum(item => item.Cost);
            return total + (total * 0.0775);
        }

        public async Task<Order> CancelOrderAsync(Order order)
        {
            try
            {
                Order? dborder = await _context.Orders.FirstOrDefaultAsync(o => o.OrderNumber == order.OrderNumber);
                if (dborder == null) { throw new NullReferenceException(order.OrderNumber); }
                dborder.Status = nameof(OrderStatusEnum.Canceled);
                _context.Orders.Update(dborder);
                await _context.SaveChangesAsync();

                return order;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

        public Task<Order> ChangeOrderAsync(Order order, PZUser pZUser, OrderModification modification)
        {
            //Add Change information to OrderModifications


            throw new NotImplementedException();
        }

        public async Task<Order> ChangeOrderStatusAsync(Order order, OrderStatus status)
        {
            try
            {
                Order? dborder = await _context.Orders.FirstOrDefaultAsync(o => o.OrderNumber == order.OrderNumber);
                if (dborder == null) { throw new NullReferenceException(order.OrderNumber); }
                dborder.Status = status.ToString();
                _context.Orders.Update(dborder);
                await _context.SaveChangesAsync();
                return order;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                throw;
            }
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {

            try
            {
                // save record and get ID
                var dborder = await _context.AddAsync(order);
                await _context.SaveChangesAsync();
                int recordId = order.Id;
                if (recordId < 99)
                {
                    string filler = "";
                    if (recordId < 10)
                    {
                        filler = "00";
                    }
                    else if (recordId > 9)
                    {
                        filler = "0";
                    }

                    order.OrderNumber = $"ORD{filler}";
                }
                else
                {
                    order.OrderNumber = $"ORD{recordId}";
                }
                await UpdateOrderAsync(order);

                return order;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                throw;
            }
        }

        public Task<List<Order>> GetActiveOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            try
            {
                List<Order> orders = await _context.Orders
                    .Include(o => o.OrderedItems)
                    .ThenInclude(o => o.Ingredients)
                    .ToListAsync();

                orders = orders.OrderByDescending(order => order.OrderDateTime).ToList();
                return orders;
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Getting Orders.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                return null!;
            }


        }

        public async Task<Order> GetCustomerOrderAsync(string orderNumber, PZUser user)
        {
            Order? order = await _context.Orders
                .Include(o => o.OrderedItems)
                    .ThenInclude(o => o.Ingredients).
                    FirstOrDefaultAsync(o => o.OrderNumber == orderNumber.ToUpper());
            if (order is null)
            {
                return null!;
            }

            if (await _userManager.IsInRoleAsync(user, "Customer"))
            {
                if (order.PZUserId == user.Id)
                {
                    return order;
                }
                else
                {
                    return null!;
                }
            }
            return order;



        }

        public Task<List<Order>> GetCustomerOrdersAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            try
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return order;
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
