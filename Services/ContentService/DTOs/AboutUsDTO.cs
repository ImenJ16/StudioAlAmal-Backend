using System.ComponentModel.DataAnnotations;

namespace ContentService.DTOs;

public class AboutUsUpdateDto
{
    [Required]
    public string Content { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }
}

public class AboutUsResponseDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public DateTime UpdatedAt { get; set; }
}