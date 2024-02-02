using Domain.Entities;

namespace Application.Services;

public interface IProductSercice
{
    Task<List<Product>> List();
    Task<Product?> GetById(int id);
    Task<Product> Create(Product newProduct);
    Task<Product> Update(Product updateProduct);
    Task Delete(int id);
}
public class ProductService : IProductSercice
{
    public Task<Product> Create(Product newProduct)
    {
        throw new NotImplementedException();
    }

    public Task<Product?> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Product>> List()
    {
        throw new NotImplementedException();
    }

    public Task<Product> Update(Product updateProduct)
    {
        throw new NotImplementedException();
    }
    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }
}
