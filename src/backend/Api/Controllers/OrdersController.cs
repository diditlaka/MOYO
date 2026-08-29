namespace Api.Controllers;

using System.Security.Claims;
using Api.Modules.Orders;
using Api.Modules.Orders.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Must be logged in to place or view orders
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // GET /api/orders
    // Returns all orders belonging to the logged in client
    [HttpGet]
    public async Task<IActionResult> GetMyOrders()
    {
        // We get the client's ID from their JWT token
        // Remember we embedded the ClientId in the token when they logged in
        var clientId = GetClientId();
        if (clientId == null)
            return Unauthorized();

        var orders = await _orderService.GetClientOrdersAsync(clientId.Value);
        return Ok(orders);
    }

    // POST /api/orders
    // Places a new order for the logged in client
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
    {
        var clientId = GetClientId();
        if (clientId == null)
            return Unauthorized();

        var order = await _orderService.CreateOrderAsync(clientId.Value, request);
        return CreatedAtAction(nameof(GetMyOrders), new { id = order.OrderId }, order);
    }

    // Helper method — reads the ClientId from the JWT token claims
    // This is how we know which client is making the request
    // without them having to send their ID manually
    private int? GetClientId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }
}