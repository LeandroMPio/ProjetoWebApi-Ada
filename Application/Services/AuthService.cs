using Domain.Entities;
using Domain.Exceptions;
using Domain.Requests;
using Domain.Responses;
using Infrastructure.Repositories;

namespace Application.Services;

public interface IAuthService
{
    Task<AuthResponse> SignIn(AuthRequest request);
}

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IHashingService _hashingService;
    private readonly IJwtService _jwtService;

    private const string InvalidLoginMessage = "Login is invalid";

    public AuthService(IUserRepository userRepository, IHashingService hashingService, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _hashingService = hashingService;
        _jwtService = jwtService;
    }
    public async Task<AuthResponse> SignIn(AuthRequest request)
    {
        var user = await _userRepository.FindByEmail(request.Email);

        if (user is null) throw new UnathorizedException(InvalidLoginMessage);

        bool passwordIsValid = _hashingService.Verify(request.Password!, user.Password!);

        if (!passwordIsValid) throw new UnathorizedException(InvalidLoginMessage);

        var jwt = _jwtService.CreateToken(user);
        return new AuthResponse
        {
            Token = jwt,
        };
    }
}
