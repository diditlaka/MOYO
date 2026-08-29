namespace Api.Modules.Orders;

using Api.Modules.Orders.Models;

public interface IOrderService
{
    Task<List<Order>> GetClientOrdersAsync(int clientId);
    Task<Order> CreateOrderAsync(int clientId, CreateOrderRequest request);
}