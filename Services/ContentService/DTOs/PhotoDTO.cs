using System.ComponentModel.DataAnnotations;

namespace ContentService.DTOs;

public class PhotoCreateDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public string ImageUrl { get; set; } = string.Empty;

    public string? ThumbnailUrl { get; set; }

    public string? Category { get; set; }

    public int DisplayOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;
}

public class PhotoUpdateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Category { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsActive { get; set; }
}

public class PhotoResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? Category { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}