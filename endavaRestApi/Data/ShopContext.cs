using Microsoft.EntityFrameworkCore;



namespace endavaRestApi.Data
{
    public class ShopContext :DbContext
    {
        public ShopContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public DbSet<User> Users { get; set; }
        public DbSet<Payment> Payments { get; set; } = null!;
        /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         {
             optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB; initial catalog=ShopDB; integrated security=True;");
         }
        */
    }
}
