using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContentService.Data;
using ContentService.DTOs;
using ContentService.Models;
using System.Security.Claims;

namespace ContentService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PhotosController : ControllerBase
{
    private readonly ContentDbContext _context;

    public PhotosController(ContentDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PhotoResponseDto>>> GetPhotos(
        [FromQuery] bool? activeOnly = null,
        [FromQuery] string? category = null)
    {
        var query = _context.Photos.AsQueryable();

        if (activeOnly == true)
        {
            query = query.Where(p => p.IsActive);
        }

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(p => p.Category == category);
        }

        var photos = await query
            .OrderBy(p => p.DisplayOrder)
            .Select(p => new PhotoResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                ThumbnailUrl = p.ThumbnailUrl,
                Category = p.Category,
                DisplayOrder = p.DisplayOrder,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();

        return Ok(photos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PhotoResponseDto>> GetPhoto(int id)
    {
        var photo = await _context.Photos.FindAsync(id);

        if (photo == null)
        {
            return NotFound(new { message = "Photo not found" });
        }

        return Ok(new PhotoResponseDto
        {
            Id = photo.Id,
            Title = photo.Title,
            Description = photo.Description,
            ImageUrl = photo.ImageUrl,
            ThumbnailUrl = photo.ThumbnailUrl,
            Category = photo.Category,
            DisplayOrder = photo.DisplayOrder,
            IsActive = photo.IsActive,
            CreatedAt = photo.CreatedAt
        });
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PhotoResponseDto>> CreatePhoto(PhotoCreateDto photoDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        var photo = new Photo
        {
            Title = photoDto.Title,
            Description = photoDto.Description,
            ImageUrl = photoDto.ImageUrl,
            ThumbnailUrl = photoDto.ThumbnailUrl,
            Category = photoDto.Category,
            DisplayOrder = photoDto.DisplayOrder,
            IsActive = photoDto.IsActive,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };

        _context.Photos.Add(photo);
        await _context.SaveChangesAsync();

        var response = new PhotoResponseDto
        {
            Id = photo.Id,
            Title = photo.Title,
            Description = photo.Description,
            ImageUrl = photo.ImageUrl,
            ThumbnailUrl = photo.ThumbnailUrl,
            Category = photo.Category,
            DisplayOrder = photo.DisplayOrder,
            IsActive = photo.IsActive,
            CreatedAt = photo.CreatedAt
        };

        return CreatedAtAction(nameof(GetPhoto), new { id = photo.Id }, response);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePhoto(int id, PhotoUpdateDto photoDto)
    {
        var photo = await _context.Photos.FindAsync(id);

        if (photo == null)
        {
            return NotFound(new { message = "Photo not found" });
        }

        if (photoDto.Title != null) photo.Title = photoDto.Title;
        if (photoDto.Description != null) photo.Description = photoDto.Description;
        if (photoDto.ImageUrl != null) photo.ImageUrl = photoDto.ImageUrl;
        if (photoDto.ThumbnailUrl != null) photo.ThumbnailUrl = photoDto.ThumbnailUrl;
        if (photoDto.Category != null) photo.Category = photoDto.Category;
        if (photoDto.DisplayOrder.HasValue) photo.DisplayOrder = photoDto.DisplayOrder.Value;
        if (photoDto.IsActive.HasValue) photo.IsActive = photoDto.IsActive.Value;
        photo.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePhoto(int id)
    {
        var photo = await _context.Photos.FindAsync(id);

        if (photo == null)
        {
            return NotFound(new { message = "Photo not found" });
        }

        _context.Photos.Remove(photo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}