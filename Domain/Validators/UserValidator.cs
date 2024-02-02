using Domain.Requests;
using Domain.Responses;

namespace Domain.Validators;

public class UserValidator : IValidator<BaseUserRequest>
{
    public List<ErrorMessageResponse> Validate(BaseUserRequest user)
    {
        var errors = new List<ErrorMessageResponse>();

        if (string.IsNullOrEmpty(user.Name))
            errors.Add(new ErrorMessageResponse
            {
                Field = "Name",
                Message = "Name is required!"
            });

        if (string.IsNullOrEmpty(user.Email))
            errors.Add(new ErrorMessageResponse
            {
                Field = "Email",
                Message = "Email is required!"
            });

        if (string.IsNullOrEmpty(user.Password))
            errors.Add(new ErrorMessageResponse
            {
                Field = "Password",
                Message = "Password is required!"
            });

        if (string.IsNullOrEmpty(user.Role))
            errors.Add(new ErrorMessageResponse
            {
                Field = "Role",
                Message = "Role is required!"
            });

        return errors;
    }
}
