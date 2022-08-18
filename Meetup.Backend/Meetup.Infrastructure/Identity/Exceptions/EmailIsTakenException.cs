namespace Meetup.Infrastructure.Identity.Exceptions;

public class EmailIsTakenException : Exception
{
    public EmailIsTakenException(string message) : base(message){}
}