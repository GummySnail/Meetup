namespace Meetup.Core.Exceptions;

public class SaveChangesToDbException : Exception
{
    public SaveChangesToDbException(string message) : base(message) {}
}