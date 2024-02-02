using Domain.Entities;
using Domain.Requests;
using Domain.Responses;

namespace Domain.Mappers;

public class ProductMapper
{
    public static ProductResponse ToResponse(Product product) => new ProductResponse
    {
        Id = product.Id,
        Name = product.Name,
        Brand = product.Brand,
        Price = product.Price,
    };

    public static Product toEntity(BaseProductRequest product) => new Product
    {
        Name = product.Name,
        Brand = product.Brand,
        Price = product.Price,
    };

    public static Product toEntity(UpdateProductRequest product) => new Product
    {
        Id = product.id,
        Name = product.Name,
        Brand = product.Brand,
        Price = product.Price,
    };
}
