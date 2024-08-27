using Microsoft.EntityFrameworkCore;

namespace SAOnlineMart.Backend.Data
{
    public class SAOnlineMartContext : DbContext
    {
        public SAOnlineMartContext(DbContextOptions<SAOnlineMartContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    }

}
