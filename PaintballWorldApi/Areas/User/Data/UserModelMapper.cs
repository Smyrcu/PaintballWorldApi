using PaintballWorld.API.Areas.User.Models;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.User.Data;

public static class UserModelMapper
{
    public static UserProfileDto Map(this UserInfo userInfo)
    {
        var result = new UserProfileDto
        {
            FirstName = userInfo.FirstName,
            LastName = userInfo.LastName,
            DateOfBirth = userInfo.DateOfBirth,
            Description = userInfo.Description,
            PhoneNo = userInfo.PhoneNo
        };
        return result;

    }

    public static UserInfo Map(this UserProfileDto dto, string userId)
    {
        var result = new UserInfo
        {
            UserId = userId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            Description = dto.Description,
            PhoneNo = dto.PhoneNo,
        };
        return result;
    }
    
    
}