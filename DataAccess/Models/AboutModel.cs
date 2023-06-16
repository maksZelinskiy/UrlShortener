namespace DataAccess.Models;

public class AboutModel
{
    public int Id { get; set; }

    public string Topic { get; set; } = null!;
    public string Content { get; set; } = null!;
}
