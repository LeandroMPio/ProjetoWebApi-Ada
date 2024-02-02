using Domain.Responses;

namespace Domain.Validators
{
    public interface IValidator<T>
    {
        public List<ErrorMessageResponse> Validate(T obj);
    }
}
