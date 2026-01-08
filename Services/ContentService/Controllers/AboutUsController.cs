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
public class AboutUsController : ControllerBase
{
    private readonly ContentDbContext _context;

    public AboutUsController(ContentDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<AboutUsResponseDto>> GetAboutUs()
    {
        var aboutUs = await _context.AboutUs.FirstOrDefaultAsync();

        if (aboutUs == null)
        {
            return NotFound(new { message = "About Us content not found" });
        }

        return Ok(new AboutUsResponseDto
        {
            Id = aboutUs.Id,
            Content = aboutUs.Content,
            ImageUrl = aboutUs.ImageUrl,
            UpdatedAt = aboutUs.UpdatedAt
        });
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateAboutUs(AboutUsUpdateDto aboutUsDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var aboutUs = await _context.AboutUs.FirstOrDefaultAsync();

        if (aboutUs == null)
        {
            // Create if doesn't exist
            aboutUs = new AboutUs
            {
                Content = aboutUsDto.Content,
                ImageUrl = aboutUsDto.ImageUrl,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = userId
            };
            _context.AboutUs.Add(aboutUs);
        }
        else
        {
            // Update existing
            aboutUs.Content = aboutUsDto.Content;
            aboutUs.ImageUrl = aboutUsDto.ImageUrl;
            aboutUs.UpdatedAt = DateTime.UtcNow;
            aboutUs.UpdatedBy = userId;
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }
}