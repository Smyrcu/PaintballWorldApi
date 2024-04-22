using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PaintballWorld.API.Areas.Schedule.Data;
using PaintballWorld.API.Areas.Schedule.Models;
using PaintballWorld.API.BaseModels;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Schedule.Controllers;


[Route("api/[area]/[controller]")]
[ApiController]
[Area("Schedule")]
[AllowAnonymous]
public class ScheduleController(
    ILogger<ScheduleController> logger,
    IAuthTokenService authTokenService,
    IScheduleService scheduleService)
    : Controller
{
   private readonly ILogger<ScheduleController> _logger = logger;

   /// <summary>
   /// Stwórz terminarz
   /// </summary>
   /// <param name="dto"></param>
   /// <param name="fieldId"></param>
   /// <returns></returns>
   [HttpPost("{fieldId}")]
   public async Task<IActionResult> CreateSchedules([FromBody] CreateSchedulesDto dto, [FromRoute]Guid fieldId)
   {
      var fieldIdObj = new FieldId(fieldId);

      var isOwner = authTokenService.IsUserOwnerOfField(User.Claims, fieldIdObj);
      if (!isOwner.success || !isOwner.errors.IsNullOrEmpty())
      {
         return BadRequest(new ResponseBase()
         {
            Errors = [ isOwner.errors ],
            IsSuccess = false,
            Message = "Owner not found or this user is not the owner"
         });
      }

      await scheduleService.AddSchedules(dto.Map(fieldIdObj));

      return Ok();
   }

   /// <summary>
   /// pobierz terminarz dla pola
   /// </summary>
   /// <param name="fieldId"></param>
   /// <returns></returns>
   [HttpGet("{fieldId}")]
   public async Task<IActionResult> GetSchedules([FromRoute] Guid fieldId)
   {
      var fieldIdObj = new FieldId(fieldId);
      
      var isOwner = authTokenService.IsUserOwnerOfField(User.Claims, fieldIdObj);
      if (!isOwner.success || !isOwner.errors.IsNullOrEmpty())
      {
         return BadRequest(new ResponseBase()
         {
            Errors = [ isOwner.errors ],
            IsSuccess = false,
            Message = "Owner not found or this user is not the owner"
         });
      }

      var result = await scheduleService.GetSchedulesByField(fieldIdObj);

      return Ok(result);
   }

   /// <summary>
   /// usuń konkretną datę dla pola
   /// </summary>
   /// <param name="fieldId"></param>
   /// <param name="scheduleId"></param>
   /// <returns></returns>
   [HttpDelete("{fieldId}/{scheduleId}")]
   public async Task<IActionResult> DeleteSchedule([FromRoute] Guid fieldId, [FromRoute] Guid scheduleId)
   {
      var fieldIdObj = new FieldId(fieldId);
      
      var isOwner = authTokenService.IsUserOwnerOfField(User.Claims, fieldIdObj);
      if (!isOwner.success || !isOwner.errors.IsNullOrEmpty())
      {
         return BadRequest(new ResponseBase()
         {
            Errors = [ isOwner.errors ],
            IsSuccess = false,
            Message = "Owner not found or this user is not the owner"
         });
      }

      await scheduleService.DeleteSchedule(fieldIdObj, new FieldScheduleId(scheduleId));
      

      return Ok();
   }
   
}