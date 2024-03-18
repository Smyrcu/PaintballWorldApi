using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaintballWorld.API.Areas.User.Controllers;

[Route("api/[area]/[controller]")]
[ApiController]
[Area("User")]
[AllowAnonymous]
public class OwnerController : Controller
{
    
}