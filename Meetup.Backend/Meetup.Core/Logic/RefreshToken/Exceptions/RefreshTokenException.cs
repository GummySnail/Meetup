namespace Meetup.Core.Logic.RefreshToken.Exceptions;

public class RefreshTokenException : Exception
{
    public RefreshTokenException(string message) : base(message) {}
}