namespace Meetup.Core.Logic.Event.Exceptions;

public class EventOwnerException : Exception
{
    public EventOwnerException(string message) : base(message){}
}