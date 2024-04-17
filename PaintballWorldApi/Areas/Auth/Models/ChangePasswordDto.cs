namespace PaintballWorld.API.Areas.Auth.Models;
public class ChangePasswordDto
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}