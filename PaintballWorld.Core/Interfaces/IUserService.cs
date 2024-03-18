using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Interfaces;

public interface IUserService
{
    UserInfo GetUserInfo(string userId);
    void UpdateProfile(UserInfo map);
}