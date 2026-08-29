namespace Api.Modules.Auth.Models;

using Api.Modules.Orders.Models; // Add this so Client can find the Order class

public class Client
{
    public int ClientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // One client can have many orders
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}