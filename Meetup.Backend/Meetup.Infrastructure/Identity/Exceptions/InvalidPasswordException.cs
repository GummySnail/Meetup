namespace Meetup.Infrastructure.Identity.Exceptions;

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException(string message) : base(message) {}
}