using Application.Services;
using Domain.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productSercice)
    {
        _productService = productSercice;
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var products = await _productService.List();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var product = await _productService.GetById(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BaseProductRequest product)
    {
        var newProduct = await _productService.Create(product);
        return Ok(newProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateProductRequest product)
    {
        product.id = id;
        var updateProduct = await _productService.Update(product);
        return Ok(updateProduct);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _productService.Delete(id);
        return NoContent();
    }
}
