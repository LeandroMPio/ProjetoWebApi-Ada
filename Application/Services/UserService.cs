using Domain.Entities;
using Domain.Exceptions;
using Domain.Mappers;
using Domain.Requests;
using Domain.Response;
using Domain.Validators;
using Infrastructure.Repositories;
using System.Security.Claims;

namespace Application.Services;

public interface IUserService
{
    Task<List<UserResponse>> List();
    Task<UserResponse?> GetById(int id, ClaimsPrincipal requestToken);
    Task<UserResponse?> FindByEmail(string email);
    Task<UserResponse> Create(BaseUserRequest newUserRequest);
    Task<UserResponse> Update(UpdateUserRequest updateUserRequest);
    Task Delete(int id);
}
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<BaseUserRequest> _validator;
    private readonly IHashingService _hashingService;

    public UserService(IUserRepository userRepository, IValidator<BaseUserRequest> validator, IHashingService hashingService)
    {
        _userRepository = userRepository;
        _validator = validator;
        _hashingService = hashingService;
    }

    public async Task<UserResponse> Create(BaseUserRequest newUserRequest)
    {
        var errors = _validator.Validate(newUserRequest);

        if (errors.Any()) throw new BadRequestException(errors);

        var newUser = UserMapper.ToEntity(newUserRequest);

        newUser.Password = _hashingService.Hash(newUser.Password!);

        var user = await _userRepository.Create(newUser);

        return UserMapper.ToResponse(user);
    }

    public async Task<List<UserResponse>> List()
    {
        var users = await _userRepository.List();
        var response = users.Select(users => UserMapper.ToResponse(users)).ToList();
        return response;
    }

    public async Task<UserResponse?> FindByEmail(string email)
    {
        var user = await _userRepository.FindByEmail(email);
        return user is null ? null : UserMapper.ToResponse(user);
    }

    public async Task<UserResponse?> GetById(int id, ClaimsPrincipal requestToken)
    {
        var user = await _userRepository.GetById(id);

        if (user is null) return null;

        var requestTokenUserId = Convert.ToInt32(requestToken.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var requestTokenUserRole = requestToken.FindFirst(ClaimTypes.Role)!.Value;

        if (requestTokenUserRole != "Admin")
        {
            return requestTokenUserId != id ? null : UserMapper.ToResponse(user);
        }

        return UserMapper.ToResponse(user);
    }

    public async Task<UserResponse> Update(UpdateUserRequest updateUserRequest)
    {
        var errors = _validator.Validate(updateUserRequest);

        if (errors.Any()) throw new BadRequestException(errors);

        var existingUser = await _userRepository.GetById(updateUserRequest.Id);

        if (existingUser is null) throw new NotFoundException("User not found!");

        var updateUser = UserMapper.ToEntity(updateUserRequest);

        updateUser.Password = _hashingService.Hash(updateUser.Password!);

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
