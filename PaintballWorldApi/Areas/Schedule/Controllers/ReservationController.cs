using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaintballWorld.API.Areas.Schedule.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Schedule")]
    [AllowAnonymous]
    public class ReservationController : Controller
    {

    }
}
