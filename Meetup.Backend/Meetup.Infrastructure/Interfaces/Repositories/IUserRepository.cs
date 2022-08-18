using Meetup.Core.Entities;
using Meetup.Infrastructure.Identity;

namespace Meetup.Infrastructure.Interfaces.Repositories;

public interface IUserRepository
{
    Task<bool> IsUserExistByEmailAsync(string email);
    Task<bool> IsUserExistByUserNameAsync(string userName);
    Task CreateUserAsync(AuthUser authUser, string password, User user);
    Task<AuthUser> GetUserByEmailAsync(string email);
    Task<AuthUser> GetUserByUserNameAsync(string userName);
    Task<bool> CheckPasswordAsync(AuthUser authUser, string password);
}