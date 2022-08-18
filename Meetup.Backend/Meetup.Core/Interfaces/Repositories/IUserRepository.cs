using Meetup.Core.Entities;

namespace Meetup.Core.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(string userId);
}