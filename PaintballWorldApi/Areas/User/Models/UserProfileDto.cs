namespace PaintballWorld.API.Areas.User.Models;

public class UserProfileDto
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string? Description { get; set; }
    
    public string? PhoneNo { get; set; }
}