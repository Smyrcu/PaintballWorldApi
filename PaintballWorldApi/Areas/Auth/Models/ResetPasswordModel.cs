namespace PaintballWorld.API.Areas.Auth.Models
{
    public record ResetPasswordModel(string Email, string Token, string NewPassword);
}
