using Domain.Exceptions;
using Domain.Mappers;
using Domain.Requests;
using Domain.Responses;
using Domain.Validators;
using Infrastructure.Repositories;

namespace Application.Services;

public interface IProductService
{
    Task<List<ProductResponse>> List();
    Task<ProductResponse?> GetById(int id);
    Task<ProductResponse> Create(BaseProductRequest newProductRequest);
    Task<ProductResponse> Update(UpdateProductRequest updateProductRequest);
    Task Delete(int id);
}
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<BaseProductRequest> _validator;

    public ProductService(IProductRepository productRepository, IValidator<BaseProductRequest> validator)
    {
        _productRepository = productRepository;
        _validator = validator;
    }

    public async Task<ProductResponse> Create(BaseProductRequest newProductRequest)
    {
        var errors = _validator.Validate(newProductRequest);

        if (errors.Any()) throw new BadRequestException(errors);

        var newProduct = ProductMapper.ToEntity(newProductRequest);
        var product = await _productRepository.Create(newProduct);

        return ProductMapper.ToResponse(product);
    }

    public async Task<ProductResponse?> GetById(int id)
    {
        var product = await _productRepository.GetById(id);
        return product is null ? null : ProductMapper.ToResponse(product);
    }

    public async Task<List<ProductResponse>> List()
    {
        var products = await _productRepository.List();
        var response = products.Select(products => ProductMapper.ToResponse(products)).ToList();
        return response;
    }

    public async Task<ProductResponse> Update(UpdateProductRequest updateProductRequest)
    {
        var errors = _validator.Validate(updateProductRequest);

        if (errors.Any()) throw new BadRequestException(errors);

        var existingProduct = await GetById(updateProductRequest.id);

        if (existingProduct is null) throw new NotFoundException("Product not found");

        var updateProduct = ProductMapper.ToEntity(updateProductRequest);

        var product = await _productRepository.Update(updateProduct);

        return ProductMapper.ToResponse(product);

    }
    public async Task Delete(int id)
    {
        var product = await GetById(id);

        if (product is null) throw new NotFoundException("Product not found");

        await _productRepository.Delete(id);
    }
}
