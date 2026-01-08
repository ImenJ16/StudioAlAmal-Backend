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
public class VideosController : ControllerBase
{
    private readonly ContentDbContext _context;

    public VideosController(ContentDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VideoResponseDto>>> GetVideos(
        [FromQuery] bool? activeOnly = null,
        [FromQuery] string? category = null)
    {
        var query = _context.Videos.AsQueryable();

        if (activeOnly == true)
        {
            query = query.Where(v => v.IsActive);
        }

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(v => v.Category == category);
        }

        var videos = await query
            .OrderBy(v => v.DisplayOrder)
            .Select(v => new VideoResponseDto
            {
                Id = v.Id,
                Title = v.Title,
                Description = v.Description,
                VideoUrl = v.VideoUrl,
                ThumbnailUrl = v.ThumbnailUrl,
                Duration = v.Duration,
                Category = v.Category,
                DisplayOrder = v.DisplayOrder,
                IsActive = v.IsActive,
                CreatedAt = v.CreatedAt
            })
            .ToListAsync();

        return Ok(videos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VideoResponseDto>> GetVideo(int id)
    {
        var video = await _context.Videos.FindAsync(id);

        if (video == null)
        {
            return NotFound(new { message = "Video not found" });
        }

        return Ok(new VideoResponseDto
        {
            Id = video.Id,
            Title = video.Title,
            Description = video.Description,
            VideoUrl = video.VideoUrl,
            ThumbnailUrl = video.ThumbnailUrl,
            Duration = video.Duration,
            Category = video.Category,
            DisplayOrder = video.DisplayOrder,
            IsActive = video.IsActive,
            CreatedAt = video.CreatedAt
        });
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<VideoResponseDto>> CreateVideo(VideoCreateDto videoDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        var video = new Video
        {
            Title = videoDto.Title,
            Description = videoDto.Description,
            VideoUrl = videoDto.VideoUrl,
            ThumbnailUrl = videoDto.ThumbnailUrl,
            Duration = videoDto.Duration,
            Category = videoDto.Category,
            DisplayOrder = videoDto.DisplayOrder,
            IsActive = videoDto.IsActive,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };

        _context.Videos.Add(video);
        await _context.SaveChangesAsync();

        var response = new VideoResponseDto
        {
            Id = video.Id,
            Title = video.Title,
            Description = video.Description,
            VideoUrl = video.VideoUrl,
            ThumbnailUrl = video.ThumbnailUrl,
            Duration = video.Duration,
            Category = video.Category,
            DisplayOrder = video.DisplayOrder,
            IsActive = video.IsActive,
            CreatedAt = video.CreatedAt
        };

        return CreatedAtAction(nameof(GetVideo), new { id = video.Id }, response);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVideo(int id, VideoUpdateDto videoDto)
    {
        var video = await _context.Videos.FindAsync(id);

        if (video == null)
        {
            return NotFound(new { message = "Video not found" });
        }

        if (videoDto.Title != null) video.Title = videoDto.Title;
        if (videoDto.Description != null) video.Description = videoDto.Description;
        if (videoDto.VideoUrl != null) video.VideoUrl = videoDto.VideoUrl;
        if (videoDto.ThumbnailUrl != null) video.ThumbnailUrl = videoDto.ThumbnailUrl;
        if (videoDto.Duration.HasValue) video.Duration = videoDto.Duration;
        if (videoDto.Category != null) video.Category = videoDto.Category;
        if (videoDto.DisplayOrder.HasValue) video.DisplayOrder = videoDto.DisplayOrder.Value;
        if (videoDto.IsActive.HasValue) video.IsActive = videoDto.IsActive.Value;
        video.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVideo(int id)
    {
        var video = await _context.Videos.FindAsync(id);

        if (video == null)
        {
            return NotFound(new { message = "Video not found" });
        }

        _context.Videos.Remove(video);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}