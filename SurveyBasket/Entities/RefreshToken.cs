using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace SurveyBasket.Entities;

[Owned]
public class RefreshToken
{
    public String Token { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiredAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiredAt;
    public bool IsActive => RevokedAt is null && !IsExpired;
}
