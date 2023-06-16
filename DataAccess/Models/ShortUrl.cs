namespace DataAccess.Models;

public class ShortUrl
{
    public int Id { get; set; }

    public string FullUrl { get; set; } = null!;
    public string? ShortenedUrl { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }

    public string UserId { get; set; } = null!;
    public User? CreatedBy { get; set; }
}
