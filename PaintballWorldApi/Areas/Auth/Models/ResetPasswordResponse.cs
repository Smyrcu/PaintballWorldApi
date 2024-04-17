using PaintballWorld.API.BaseModels;

namespace PaintballWorld.API.Areas.Auth.Models
{
    public class ResetPasswordResponse : ResponseBase
    {
        public string NewPassword { get; set; }
    }
}
