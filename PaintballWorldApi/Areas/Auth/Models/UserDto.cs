namespace PaintballWorld.API.Areas.Auth.Models;

public class UserDto
{
    public UserDto()
    {
        
    }
    public UserDto(string email, string username, string password, DateTime dateOfBirth)
    {
        this.Email = email;
        this.Username = username;
        this.Password = password;
        this.DateOfBirth = dateOfBirth;
    }

    public string Email { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
    public DateTime DateOfBirth { get; init; }

    public void Deconstruct(out string email, out string username, out string password, out DateTime dateOfBirth)
    {
        email = this.Email;
        username = this.Username;
        password = this.Password;
        dateOfBirth = this.DateOfBirth;
    }
}
