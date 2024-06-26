﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Infrastructure.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using PaintballWorld.API.Areas.Field.Models;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Interfaces;

namespace PaintballWorld.API.Areas.Field.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Field")]
    [AllowAnonymous]
    public class FieldManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IFieldManagementService _fieldManagementService;
        private readonly IAuthTokenService _authTokenService;
        private readonly ILogger<FieldManagementController> _logger;


        public FieldManagementController(IFieldManagementService fieldManagementService, UserManager<IdentityUser> userManager, ApplicationDbContext context, IAuthTokenService authTokenService, ILogger<FieldManagementController> logger)
        {
            this._fieldManagementService = fieldManagementService;
            _userManager = userManager;
            _context = context;
            _authTokenService = authTokenService;
            _logger = logger;
        }

        #region Photo
        
        /// <summary>
        /// Dodaj zdjęcia do pola
        /// </summary>
        /// <param name="photos"></param>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        [HttpPost("photos/{fieldId:guid}")]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public async Task<IActionResult> AddPhotos([FromForm]IFormFileCollection photos, [FromRoute]Guid fieldId)
        {
            var username = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            if (username == null)
                return BadRequest("Wrong JWT");

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return BadRequest("User not found");

            var owner = _context.Owners.FirstOrDefault(x => x.UserId == Guid.Parse(user.Id));

            if(owner == null)
                return BadRequest("This account is not Owner");
            var parsedFieldId = new FieldId(fieldId);

            var fieldIds = _context.Fields.FirstOrDefault(x => x.OwnerId == owner.Id && x.Id == parsedFieldId);
            
            if(fieldIds == null)
                return BadRequest("This user is not the Owner of this field.");

            var errors = new List<string>();

            foreach (var photo in photos)
            {
                if (photo.Length > 0)
                {
                    var extension = Path.GetExtension(photo.FileName).ToLower();
                    if (extension != ".jpg" && extension != ".png" && extension != ".jpeg")
                    {
                        errors.Add($"{photo.FileName} - Invalid file type. Only JPG and PNG are allowed.");
                    }
                    else
                    {
                        try
                        {
                            using var stream = new MemoryStream();
                            await photo.CopyToAsync(stream);
                            stream.Position = 0;
                            var fId = new FieldId(fieldId);
                            _fieldManagementService.SavePhoto(stream, fId);
                        }
                        catch (Exception ex)
                        {
                            errors.Add($"Error uploading {photo.FileName}: {ex.Message}");
                        }
                    }
                }
                else
                {
                    errors.Add($"{photo.FileName} is empty and cannot be uploaded.");
                }
            }
            var message = errors.Any()
                ? "Photos uploaded with errors"
                : "Photos uploaded successfully.";

            return Ok(new { Message = message, Errors = errors});
        }

        /// <summary>
        /// usuń zdjęcie
        /// </summary>
        /// <param name="photoId"></param>
        /// <returns></returns>
        [HttpDelete("photos/{photoId:guid}")]
        public async Task<IActionResult> DeletePhoto([FromRoute] Guid photoId)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(x => x.Id == new PhotoId(photoId));
            if (photo?.FieldId == null)
            {
                return BadRequest("Photo does not exist or it is not field photo.");
            }
            var isOwner = _authTokenService.IsUserOwnerOfField(User.Claims,photo.FieldId.Value);
            
            if (!isOwner.success || !isOwner.errors.IsNullOrEmpty())
            {
                return BadRequest(new DeletePhotosResponse()
                {
                    Errors = [ isOwner.errors ],
                    IsSuccess = false,
                    Message = "Owner not found or this user is not the owner"
                });
            }
            _context.Photos.Remove(photo);
            Photo? photo2;
            if(photo.Path.Contains(".300.jpg"))
            {
                photo2 = await _context.Photos.FirstOrDefaultAsync(x => x.Path == photo.Path.Replace(".300.jpg", ".jpg"));
                
            }
            else
            {
                photo2 =
                    await _context.Photos.FirstOrDefaultAsync(x => x.Path == photo.Path.Replace(".jpg", ".300.jpg"));
            }

            if (photo2 != null)
            {
                _context.Photos.Remove(photo2);
            }
            await _context.SaveChangesAsync();

            try
            {
                System.IO.File.Delete(Path.Combine(Constants.BasePath, photo.Path));
                System.IO.File.Delete(Path.Combine(Constants.BasePath, photo2.Path));
            }
            catch (Exception ex)
            {
                // olać jak się nie udało
            }

            return Ok("Photo deleted successfully");
        }

        /// <summary>
        /// pobierz zdjęcia dla pola
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        [HttpGet("photos/{fieldId:guid}")]
        public async Task<IActionResult> GetPhotos([FromRoute] Guid fieldId)
        {
            var id = new FieldId(fieldId);
            // var isOwner = _authTokenService.IsUserOwnerOfField(User.Claims,id);
            //
            // if (!isOwner.success || !isOwner.errors.IsNullOrEmpty())
            // {
            //     return BadRequest(new DeletePhotosResponse()
            //     {
            //         Errors = [ isOwner.errors ],
            //         IsSuccess = false,
            //         Message = "Owner not found or this user is not the owner"
            //     });
            // }

            var photos = _context.Photos.Where(x => x.FieldId == id).Take(20);

            var result = await photos.Select(x => new { url = $"{Request.Scheme}://{Request.Host}/img/{x.Path}", Id = x.Id }).ToListAsync();
            
            return Ok(result);
        }
        
        #endregion
        



    }
}
