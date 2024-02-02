using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public interface IProductRepository
{
    Task<List<Product>> List();
    Task<Product?> GetById(int id);
    Task<Product> Create(Product newProduct);
    Task<Product> Update(Product updateProduct);
    Task Delete(int id);
}
public class ProductRepository : IProductRepository
{
    private readonly Context _context;

    public ProductRepository(Context context)
    {
        _context = context;
    }

    public async Task<Product> Create(Product newProduct)
    {
        await _context.Products.AddAsync(newProduct);
        await _context.SaveChangesAsync();

        return newProduct;
    }

    public async Task<Product?> GetById(int id)
    {
        return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Product>> List()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> Update(Product updateProduct)
    {
        var product = await GetById(updateProduct.Id);

        if (product is null) throw new Exception("Product not found");

        product.Name = updateProduct.Name;
        product.Brand = updateProduct.Brand;
        product.Price = updateProduct.Price;

        _context.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task Delete(int id)
    {
        var product = await GetById(id);
        if (product is null) throw new Exception("Product not found");

        _context.Remove(product);
        await _context.SaveChangesAsync();
    }
}
