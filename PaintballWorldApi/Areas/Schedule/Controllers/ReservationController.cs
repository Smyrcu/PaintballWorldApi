using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.Areas.Schedule.Data;
using PaintballWorld.API.Areas.Schedule.Models;
using PaintballWorld.API.BaseModels;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Core.Services;
using PaintballWorld.Infrastructure;

namespace PaintballWorld.API.Areas.Schedule.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Schedule")]
    [AllowAnonymous]
    public class ReservationController(IReservationService reservationService) : Controller
    {


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        public async Task<IActionResult> GetReservations()
        {
            throw new NotImplementedException();
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

        [HttpPut]
        public async Task<IActionResult> EditReservation()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteReservation()
        {
            throw new NotImplementedException();
        }
    }
}
