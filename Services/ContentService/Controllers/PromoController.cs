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
public class PromosController : ControllerBase
{
    private readonly ContentDbContext _context;

    public PromosController(ContentDbContext context)
    {
        _context = context;
    }

    // GET: api/Promos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PromoResponseDto>>> GetPromos([FromQuery] bool? activeOnly = null)
    {
        var query = _context.Promos.AsQueryable();

        if (activeOnly == true)
        {
            query = query.Where(p => p.IsActive);
        }

        var promos = await query
            .OrderBy(p => p.DisplayOrder)
            .Select(p => new PromoResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                DisplayOrder = p.DisplayOrder,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .ToListAsync();

        return Ok(promos);
    }

    // GET: api/Promos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PromoResponseDto>> GetPromo(int id)
    {
        var promo = await _context.Promos.FindAsync(id);

        if (promo == null)
        {
            return NotFound(new { message = "Promo not found" });
        }

        return Ok(new PromoResponseDto
        {
            Id = promo.Id,
            Title = promo.Title,
            Description = promo.Description,
            ImageUrl = promo.ImageUrl,
            DisplayOrder = promo.DisplayOrder,
            IsActive = promo.IsActive,
            CreatedAt = promo.CreatedAt,
            UpdatedAt = promo.UpdatedAt
        });
    }

    // POST: api/Promos
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PromoResponseDto>> CreatePromo(PromoCreateDto promoDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        var promo = new Promo
        {
            Title = promoDto.Title,
            Description = promoDto.Description,
            ImageUrl = promoDto.ImageUrl,
            DisplayOrder = promoDto.DisplayOrder,
            IsActive = promoDto.IsActive,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };

        _context.Promos.Add(promo);
        await _context.SaveChangesAsync();

        var response = new PromoResponseDto
        {
            Id = promo.Id,
            Title = promo.Title,
            Description = promo.Description,
            ImageUrl = promo.ImageUrl,
            DisplayOrder = promo.DisplayOrder,
            IsActive = promo.IsActive,
            CreatedAt = promo.CreatedAt
        };

        return CreatedAtAction(nameof(GetPromo), new { id = promo.Id }, response);
    }

    // PUT: api/Promos/5
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePromo(int id, PromoUpdateDto promoDto)
    {
        var promo = await _context.Promos.FindAsync(id);

        if (promo == null)
        {
            return NotFound(new { message = "Promo not found" });
        }

        if (promoDto.Title != null) promo.Title = promoDto.Title;
        if (promoDto.Description != null) promo.Description = promoDto.Description;
        if (promoDto.ImageUrl != null) promo.ImageUrl = promoDto.ImageUrl;
        if (promoDto.DisplayOrder.HasValue) promo.DisplayOrder = promoDto.DisplayOrder.Value;
        if (promoDto.IsActive.HasValue) promo.IsActive = promoDto.IsActive.Value;
        promo.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Promos/5
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePromo(int id)
    {
        var promo = await _context.Promos.FindAsync(id);

        if (promo == null)
        {
            return NotFound(new { message = "Promo not found" });
        }

        _context.Promos.Remove(promo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}