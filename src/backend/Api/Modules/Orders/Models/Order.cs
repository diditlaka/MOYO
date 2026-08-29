namespace Api.Modules.Orders.Models;

using Api.Modules.Auth.Models;
using Api.Modules.Products.Models;

// This class represents a row in the Order table
public class Order
{
    public int OrderId { get; set; }

    // Foreign key — links this order to a specific client
    public int ClientId { get; set; }

    // Foreign key — links this order to a specific product
    public int ProductId { get; set; }

    public int Quantity { get; set; } = 1;

    // Status tracks where the order is in its lifecycle
    // Pending → Processing → Shipped → Delivered
    public string Status { get; set; } = "Pending";

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    // Navigation properties — let us access the full Client
    // and Product objects directly from an Order object
    public Client Client { get; set; } = null!;
    public Product Product { get; set; } = null!;
}