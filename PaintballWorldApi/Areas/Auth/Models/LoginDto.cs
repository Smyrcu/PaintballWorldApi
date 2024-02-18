namespace PaintballWorld.API.Areas.Auth.Models
{
    public class LoginDto
    {
        public LoginDto(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public string Username { get; init; }
        public string Password { get; init; }

        public void Deconstruct(out string username, out string password)
        {
            username = this.Username;
            password = this.Password;
        }
    }
}
