using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaintballWorld.API.Areas.Field.Data;
using PaintballWorld.API.Areas.Field.Models;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Field.Controllers;

[Route("api/[area]/[controller]")]
[ApiController]
[Area("Field")]
[AllowAnonymous]
public class SetsController : Controller
{
    private readonly IAuthTokenService _authTokenService;
    private readonly ApplicationDbContext _context;

    public SetsController(IAuthTokenService authTokenService, ApplicationDbContext context)
    {
        _authTokenService = authTokenService;
        _context = context;
    }

    /// <summary>
    /// Dodaj sety do pola
    /// </summary>
    /// <param name="sets"></param>
    /// <param name="fieldId"></param>
    /// <returns></returns>
    [HttpPost("{fieldId}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> AddSets([FromBody] IList<SetDto> sets, [FromRoute]Guid fieldId)
    {
        var fieldIdModel = new FieldId(fieldId);

        var isOwner = _authTokenService.IsUserOwnerOfField(User.Claims, fieldIdModel);
        if (!isOwner.success || !isOwner.errors.IsNullOrEmpty())
        {
            return BadRequest(new AddSetsResponse
            {
                Errors = [ isOwner.errors ],
                IsSuccess = false,
                Message = "Owner not found"
            });
        }
        var setModel = sets.Select(x => x.Map(fieldIdModel));

        await _context.Sets.AddRangeAsync(setModel);
        await _context.SaveChangesAsync();
        var value = new AddSetsResponse
        {
            IsSuccess = true,
            Message = "Sets added successfully"
        };
        return Ok(value);

    }

    /// <summary>
    /// pobierz sety dla pola
    /// </summary>
    /// <param name="fieldId"></param>
    /// <returns></returns>
    [HttpGet("{fieldId}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> GetSets([FromRoute] Guid fieldId)
    {
        var fieldIdModel = new FieldId(fieldId);
        var isOwner = _authTokenService.IsUserOwnerOfField(User.Claims, fieldIdModel);
        if (!isOwner.success || !isOwner.errors.IsNullOrEmpty())
        {
            return BadRequest(new AddSetsResponse
            {
                Errors = [ isOwner.errors ],
                IsSuccess = false,
                Message = "Owner not found"
            });
        }
        var sets = await _context.Sets.Where(s => s.FieldId.Value == fieldId).ToListAsync();
        var setDtos = sets.Map();
        return Ok(setDtos);
    }
    
    /// <summary>
    /// edytuj sety dla pola
    /// </summary>
    /// <param name="sets"></param>
    /// <param name="fieldId"></param>
    /// <returns></returns>
    [HttpPut("{fieldId}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> ManageSets([FromBody] IList<SetDto> sets, [FromRoute] Guid fieldId)
    {
        var fieldIdModel = new FieldId(fieldId);
        var isOwner = _authTokenService.IsUserOwnerOfField(User.Claims, fieldIdModel);
        if (!isOwner.success || !isOwner.errors.IsNullOrEmpty())
        {
            return BadRequest(new AddSetsResponse
            {
                Errors = [ isOwner.errors ],
                IsSuccess = false,
                Message = "Owner not found"
            });
        }
        var fieldSets = await _context.Sets.Where(s => s.FieldId.Value == fieldId).ToListAsync();
    
        if (fieldSets.Count == 0)
        {
            return NotFound("Sets not found for the provided field ID.");
        }

        foreach (var setDto in sets)
        {
            var set = fieldSets.FirstOrDefault(s => s.Id.Value == setDto.Id);
            set?.UpdateFromDto(setDto);
        }

        _context.Sets.UpdateRange(fieldSets);
        await _context.SaveChangesAsync();
        return Ok("Sets updated successfully");
    }

    /// <summary>
    /// usuń set dla pola
    /// </summary>
    /// <param name="setId"></param>
    /// <param name="fieldId"></param>
    /// <returns></returns>
    [HttpDelete("{fieldId}/{setId}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> DeleteSet([FromRoute] Guid setId, [FromRoute] Guid fieldId)
    {
        var fieldIdModel = new FieldId(fieldId);
        var isOwner = _authTokenService.IsUserOwnerOfField(User.Claims, fieldIdModel);
        if (!isOwner.success || !isOwner.errors.IsNullOrEmpty())
        {
            return BadRequest(new AddSetsResponse
            {
                Errors = [ isOwner.errors ],
                IsSuccess = false,
                Message = "Owner not found"
            });
        }
        var set = await _context.Sets.FindAsync(setId);
        if (set == null)
        {
            return NotFound("Set not found");
        }

        _context.Sets.Remove(set);
        await _context.SaveChangesAsync();
        return Ok("Set deleted successfully");
    }

    
}