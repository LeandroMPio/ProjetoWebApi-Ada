using Domain.Requests;
using Domain.Responses;

namespace Domain.Validators;

public class ProductValidator : IValidator<BaseProductRequest>
{
    public List<ErrorMessageResponse> Validate(BaseProductRequest product)
    {
        var errors = new List<ErrorMessageResponse>();

        if (string.IsNullOrEmpty(product.Name))
            errors.Add(new ErrorMessageResponse
            {
                Field = "Name",
                Message = "Product name is required"
            });

        if (string.IsNullOrEmpty(product.Brand))
            errors.Add(new ErrorMessageResponse
            {
                Field = "Brand",
                Message = "Product brand is required"
            });

        if (!product.Price.HasValue)
            errors.Add(new ErrorMessageResponse
            {
                Field = "Price",
                Message = "Product Price is required"
            });

        return errors;
    }
}
