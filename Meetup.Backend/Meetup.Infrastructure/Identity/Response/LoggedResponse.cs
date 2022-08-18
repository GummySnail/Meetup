namespace Meetup.Infrastructure.Identity.Response;

public record LoggedResponse(string Email, string UserName, string AccessToken/*, string RefreshToken*/);