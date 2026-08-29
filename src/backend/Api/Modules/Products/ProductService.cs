namespace Api.Modules.Products;

using Api.Infrastructure.Persistence;
using Api.Modules.Products.Models;
using Microsoft.EntityFrameworkCore;

// ProductService handles reading products from the database
// In the full system products would come from the Product Management System
// For now we read them from our local database
public class ProductService : IProductService
{
    private readonly AppDbContext _db;

    public ProductService(AppDbContext db)
    {
        _db = db;
    }

    // Get all available products
    public async Task<List<Product>> GetAllAsync()
    {
        return await _db.Products
            .Where(p => p.IsAvailable)
            .ToListAsync();
    }

    // Get a single product by its ID
    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _db.Products
            .FirstOrDefaultAsync(p => p.ProductId == id && p.IsAvailable);
    }
}