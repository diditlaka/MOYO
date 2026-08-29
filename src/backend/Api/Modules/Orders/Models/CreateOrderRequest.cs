namespace Api.Modules.Orders.Models;

// This is what the frontend sends when placing a new order
public class CreateOrderRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
}