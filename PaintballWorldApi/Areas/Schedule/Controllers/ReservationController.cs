using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.Areas.Schedule.Data;
using PaintballWorld.API.Areas.Schedule.Models;
using PaintballWorld.API.BaseModels;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Core.Models;
using PaintballWorld.Core.Services;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Interfaces;

namespace PaintballWorld.API.Areas.Schedule.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Schedule")]
    [AllowAnonymous]
    public class ReservationController(IReservationService reservationService, IAuthTokenService authTokenService) : Controller
    {


        /// <summary>
        /// Get Reservation/s
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        public async Task<IActionResult> GetReservations([FromQuery]Guid fieldId, [FromQuery]string? userId)
        {
            var result = reservationService.GetFieldReservations(fieldId, userId);

            return Ok(result);
        }

        /// <summary>
        /// Create reservation
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto dto)
        {
            try
            {
                await reservationService.Create(dto.Map());
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBase
                {
                    IsSuccess = false,
                    Errors = [ex.Message],
                    Message = "Creating reservation failed"
                });
            }

            
        }

        /// <summary>
        /// Edit Reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> EditReservation([FromBody] EventModel model)
        {
            try
            {
                var userId = authTokenService.GetUserId(User.Claims);
                await reservationService.EditReservation(model, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBase
                {
                    IsSuccess = false,
                    Errors = [ex.Message]
                });
            }
        }


        /// <summary>
        /// Delete reservation
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteReservation([FromQuery] Guid eventId)
        {
            var userId = authTokenService.GetUserId(User.Claims);
            try
            {
                await reservationService.DeleteReservation(eventId, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBase
                {
                    IsSuccess = false,
                    Errors = [ex.Message]
                });
            }
        }
    }
}
