namespace Meetup.Infrastructure.Identity.Exceptions;

public class UserNameIsTakenException : Exception
{
    public UserNameIsTakenException(string message) : base(message) {}
}