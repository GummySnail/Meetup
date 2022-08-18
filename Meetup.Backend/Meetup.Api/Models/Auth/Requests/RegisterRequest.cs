namespace Meetup.Api.Models.Auth.Requests;

public record RegisterRequest(string Email, string UserName, string Password, string ConfirmPassword);