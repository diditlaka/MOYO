using Microsoft.EntityFrameworkCore;
using Api.Modules.Auth.Models;
using Api.Modules.Products.Models;
using Api.Modules.Orders.Models;

namespace Api.Infrastructure.Persistence;

// AppDbContext is the bridge between our C# code and the SQL Server database
// Think of it as the "manager" that knows about all our tables
// Every time we want to read or write to the database, we go through this class
public class AppDbContext : DbContext
{
    // This constructor receives the database settings (like the connection string)
    // and passes them to the parent DbContext class
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Each DbSet represents a table in the database
    // DbSet<Client> = the Client table
    // DbSet<Product> = the Product table
    // DbSet<Order>   = the Order table
    public DbSet<Client> Clients { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }

    // OnModelCreating is where we configure the relationships between tables
    // This is the code version of the foreign keys we set up in SQL
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // One Client can have many Orders
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Client)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.ClientId);

        // One Product can appear in many Orders
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Product)
            .WithMany(p => p.Orders)
            .HasForeignKey(o => o.ProductId);

        // Tell EF that the Order table is called "Order" in SQL
        // (we need this because Order is a reserved word in SQL)
        modelBuilder.Entity<Order>().ToTable("Order");
    }
}