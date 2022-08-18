using System.ComponentModel.DataAnnotations.Schema;

namespace Meetup.Infrastructure.Identity;

[Table("AspNetUserRefreshTokens")]
public class ApplicationUserRefreshTokens
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiryTime { get; set; }
}