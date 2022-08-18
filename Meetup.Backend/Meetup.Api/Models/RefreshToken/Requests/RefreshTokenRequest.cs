namespace Meetup.Api.Models.RefreshToken.Requests;

public record RefreshTokenRequest(string AccessToken, string RefreshToken);