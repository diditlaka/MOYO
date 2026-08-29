namespace Api.Modules.Products;

using Api.Modules.Products.Models;

public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
}