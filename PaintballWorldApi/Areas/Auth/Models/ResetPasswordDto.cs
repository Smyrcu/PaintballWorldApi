namespace PaintballWorld.API.Areas.Auth.Models
{
    public class ResetPasswordDto
    {
        public ResetPasswordDto(string Email, string Token, string NewPassword)
        {
            this.Email = Email;
            this.Token = Token;
            this.NewPassword = NewPassword;
        }

        public string Email { get; init; }
        public string Token { get; init; }
        public string NewPassword { get; init; }

        public void Deconstruct(out string Email, out string Token, out string NewPassword)
        {
            Email = this.Email;
            Token = this.Token;
            NewPassword = this.NewPassword;
        }
    }
}
