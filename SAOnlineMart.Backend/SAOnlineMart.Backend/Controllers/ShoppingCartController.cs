using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAOnlineMart.Backend.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SAOnlineMart.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly SAOnlineMartContext _context;

        public ShoppingCartController(SAOnlineMartContext context)
        {
            _context = context;
        }

        // GET: api/ShoppingCart/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCart(int userId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        // POST: api/ShoppingCart/{userId}/AddItem
        [HttpPost("{userId}/AddItem")]
        public async Task<ActionResult<ShoppingCart>> AddItemToCart(int userId, ShoppingCartItem item)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new ShoppingCart { UserId = userId };
                _context.ShoppingCarts.Add(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cart.Items.Add(item);
            }

            await _context.SaveChangesAsync();
            return cart;
        }

        // PUT: api/ShoppingCart/{userId}/UpdateItem
        [HttpPut("{userId}/UpdateItem")]
        public async Task<IActionResult> UpdateCartItem(int userId, ShoppingCartItem item)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound();
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Quantity = item.Quantity;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/ShoppingCart/{userId}/RemoveItem/{productId}
        [HttpDelete("{userId}/RemoveItem/{productId}")]
        public async Task<IActionResult> RemoveItemFromCart(int userId, int productId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound();
            }

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
            {
                return NotFound();
            }

            cart.Items.Remove(item);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/ShoppingCart/{userId}/Clear
        [HttpDelete("{userId}/Clear")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound();
            }

            _context.ShoppingCarts.Remove(cart);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{userId}/Checkout")]
        public async Task<ActionResult<Order>> Checkout(int userId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.Items.Any())
            {
                return BadRequest("Shopping cart is empty or not found.");
            }

            var totalAmount = cart.Items.Sum(i => i.Product.Price * i.Quantity);

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                OrderItems = cart.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Product.Price
                }).ToList()
            };

            _context.Orders.Add(order);

            _context.ShoppingCarts.Remove(cart);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(OrdersController.GetOrder), "Orders", new { id = order.Id }, order);
        }


    }
}
