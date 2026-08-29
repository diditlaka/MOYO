namespace Api.Modules.Products.Models;

using Api.Modules.Orders.Models; // Add this so Product can find the Order class

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true;

    // One product can appear in many orders
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}