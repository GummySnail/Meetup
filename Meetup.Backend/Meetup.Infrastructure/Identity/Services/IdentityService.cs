using System.Security.Claims;
using Meetup.Core.Entities;
using Meetup.Core.Exceptions;
using Meetup.Core.Interfaces.Repositories;
using Meetup.Core.Interfaces.Services;
using Meetup.Infrastructure.Identity.Exceptions;
using Meetup.Infrastructure.Identity.Response;
using Meetup.Infrastructure.Interfaces.Repositories;

namespace Meetup.Infrastructure.Identity.Services;

public class IdentityService
{
    private readonly IAuthUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public IdentityService(IAuthUserRepository userRepository, ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<LoggedResponse> RegisterAsync(string email, string userName, string password,
        string confirmPassword)
    {
        if (await _userRepository.IsUserExistByEmailAsync(email))
        {
            throw new EmailIsTakenException($"Email '{email}' is already taken");
        }

        if (await _userRepository.IsUserExistByUserNameAsync(userName))
        {
            throw new UserNameIsTakenException($"Username '{userName}' is already taken");
        }

        if (!confirmPassword.Equals(password))
        {
            throw new PasswordsAreNotEqualException("Passwords aren't equal");
        }

        var authUser = new AuthUser
        {
            UserName = userName,
            Email = email
        };

        var user = new User
        {
            Id = authUser.Id,
            UserName = userName,
            Email = email
        };

        await _userRepository.CreateUserAsync(authUser, password, user);
        
        return await ReturnAuthorizedUser(authUser);
    }

    public async Task<LoggedResponse> LoginByEmailAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);

        if (user is null)
        {
            throw new NotFoundException($"User with email '{email}' not found");
        }

        if (!await _userRepository.CheckPasswordAsync(user, password))
        {
            throw new InvalidPasswordException("Invalid password");
        }

        return await ReturnAuthorizedUser(user);
    }

    public async Task<LoggedResponse> LoginByUserNameAsync(string userName, string password)
    {
        var user = await _userRepository.GetUserByUserNameAsync(userName);
        
        if (user is null)
        {
            throw new NotFoundException($"User with username '{userName}' not found");
        }

        if (!await _userRepository.CheckPasswordAsync(user, password))
        {
            throw new InvalidPasswordException("Invalid password");
        }

        return await ReturnAuthorizedUser(user);
    }

    private async Task<LoggedResponse> ReturnAuthorizedUser(AuthUser authUser)
    {
        var accessToken = _tokenService.GenerateAccessToken(new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, authUser.Id),
            new Claim(ClaimTypes.Role, "User")
        });

        var refreshToken = _tokenService.GenerateRefreshToken();
        await _refreshTokenRepository.AddRefreshTokenAsync(authUser.Id, refreshToken, DateTime.UtcNow.AddMonths(1));
        await _refreshTokenRepository.SaveChangesAsync();
        
        return new LoggedResponse(authUser.Email, authUser.UserName, accessToken, refreshToken);
    }
}