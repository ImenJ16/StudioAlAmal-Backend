using System.ComponentModel.DataAnnotations;

namespace ContentService.DTOs;

public class VideoCreateDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public string VideoUrl { get; set; } = string.Empty;

    public string? ThumbnailUrl { get; set; }

    public int? Duration { get; set; }

    public string? Category { get; set; }

    public int DisplayOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;
}

public class VideoUpdateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int? Duration { get; set; }
    public string? Category { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsActive { get; set; }
}

public class VideoResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string VideoUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public int? Duration { get; set; }
    public string? Category { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}