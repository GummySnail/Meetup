namespace Meetup.Core.Entities;

public class RefreshToken
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public string RefreshTokenValue { get; set; }
    public DateTime ExpiryTime { get; set; }
}