namespace Api.Controllers;

using Api.Modules.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize] // This means you must be logged in to access any endpoint in this controller
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // GET /api/products
    // Returns all available products for the client to browse
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    // GET /api/products/5
    // Returns a single product by its ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product == null)
            return NotFound(new { message = "Product not found" });

        return Ok(product);
    }
}