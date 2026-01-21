namespace ContentService.Models;

public class AboutUs
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public int UpdatedBy { get; set; }
}