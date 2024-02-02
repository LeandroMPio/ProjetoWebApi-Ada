using Domain.Exceptions;
using Domain.Mappers;
using Domain.Requests;
using Domain.Response;
using Domain.Validators;
using Infrastructure.Repositories;

namespace Application.Services;

public interface IUserService
{
    Task<List<UserResponse>> List();
    Task<UserResponse?> GetById(int id);
    Task<UserResponse?> FindByEmail(string email);
    Task<UserResponse> Create(BaseUserRequest newUserRequest);
    Task<UserResponse> Update(UpdateUserRequest updateUserRequest);
    Task Delete(int id);
}
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<BaseUserRequest> _validator;

    public UserService(IUserRepository userRepository, IValidator<BaseUserRequest> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<UserResponse> Create(BaseUserRequest newUserRequest)
    {
        var errors = _validator.Validate(newUserRequest);

        if (errors.Any()) throw new BadRequestException(errors);

        var newUser = UserMapper.ToEntity(newUserRequest);
        var user = await _userRepository.Create(newUser);

        return UserMapper.ToResponse(user);
    }

    public async Task<List<UserResponse>> List()
    {
        var user = await _userRepository.List();
        var response = user.Select(user => UserMapper.ToResponse(user)).ToList();
        return response;
    }

    public async Task<UserResponse?> FindByEmail(string email)
    {
        var user = await _userRepository.FindByEmail(email);
        return user is null ? null : UserMapper.ToResponse(user);
    }

    public async Task<UserResponse?> GetById(int id)
    {
        var user = await _userRepository.GetById(id);
        return user is null ? null : UserMapper.ToResponse(user);
    }

    public async Task<UserResponse> Update(UpdateUserRequest updateUserRequest)
    {
        var errors = _validator.Validate(updateUserRequest);

        if (errors.Any()) throw new BadRequestException(errors);

        var existingUser = await _userRepository.GetById(updateUserRequest.Id);

        if (existingUser is null) throw new NotFoundException("User not found!");

        var updateUser = UserMapper.ToEntity(updateUserRequest);

        var user = await _userRepository.Update(updateUser);

        return UserMapper.ToResponse(user);
    }
    public async Task Delete(int id)
    {
        var user = await _userRepository.GetById(id);

        if (user is null) throw new NotFoundException("User not found!");

        await _userRepository.Delete(id);
    }
}
