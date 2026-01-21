using System.ComponentModel.DataAnnotations;

namespace ContentService.DTOs;

public class PromoCreateDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public string ImageUrl { get; set; } = string.Empty;

    public int DisplayOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;
}

public class PromoUpdateDto
{
    [StringLength(200)]
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public int? DisplayOrder { get; set; }

    public bool? IsActive { get; set; }
}

public class PromoResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}