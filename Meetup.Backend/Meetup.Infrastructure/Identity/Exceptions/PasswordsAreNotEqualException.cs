namespace Meetup.Infrastructure.Identity.Exceptions;

public class PasswordsAreNotEqualException : Exception
{
    public PasswordsAreNotEqualException(string message) : base(message){}
}