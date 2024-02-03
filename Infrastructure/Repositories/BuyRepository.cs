using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IBuyRepository
{
    Task<Product> Create(Product newBuy);
}
public class BuyRepository
{
}
