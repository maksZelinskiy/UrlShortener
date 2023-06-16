using DataAccess.Models;

namespace BusinessLogic.DTOs;

public class UrlModel
{
    public ShortUrl ShortUrl { get; set; } = null!;
    public bool CanBeDeleted { get; set; }
}
