using Domain.Entities;
using Domain.Requests;
using Domain.Response;

namespace Domain.Mappers;

public class UserMapper
{
    public static UserResponse ToResponse(User user) => new UserResponse
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Role = user.Role,
    };

    public static User ToEntity(BaseUserRequest user) => new User
    {
        Name = user.Name,
        Email = user.Email,
        Password = user.Password,
        Role = user.Role,
    };

    public static User ToEntity(UpdateUserRequest user) => new User
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Password = user.Password,
        Role = user.Role,
    };
}
