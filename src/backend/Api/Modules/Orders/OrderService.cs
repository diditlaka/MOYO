namespace Api.Modules.Orders;

using Api.Infrastructure.Persistence;
using Api.Modules.Orders.Models;
using Microsoft.EntityFrameworkCore;

public class OrderService : IOrderService
{
    private readonly AppDbContext _db;

    public OrderService(AppDbContext db)
    {
        _db = db;
    }

    // Get all orders for a specific client
    // We include the Product details so the frontend can show product names
    public async Task<List<Order>> GetClientOrdersAsync(int clientId)
    {
        return await _db.Orders
            .Where(o => o.ClientId == clientId)
            .Include(o => o.Product)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    // Place a new order for a client
    public async Task<Order> CreateOrderAsync(int clientId, CreateOrderRequest request)
    {
       if (request.Quantity <= 0)
        throw new ArgumentException("Quantity must be greater than zero.");

    var product = await _db.Products
        .FirstOrDefaultAsync(p => p.ProductId == request.ProductId);

    if (product == null)
        throw new KeyNotFoundException("Product not found.");

    if (!product.IsAvailable)
        throw new InvalidOperationException("Product is currently unavailable.");

    var order = new Order
    {
        ClientId = clientId,
        ProductId = request.ProductId,
        Quantity = request.Quantity,
        Status = "Pending",
        OrderDate = DateTime.UtcNow
    };

    _db.Orders.Add(order);
    await _db.SaveChangesAsync();

    return await _db.Orders
        .Include(o => o.Product)
        .FirstAsync(o => o.OrderId == order.OrderId);
    }
}