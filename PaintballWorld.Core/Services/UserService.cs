using System.Data;
using Microsoft.Extensions.Logging;
using MimeKit.Text;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(ApplicationDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public UserInfo GetUserInfo(string userId)
    {
        var result = _context.UserInfos.FirstOrDefault(x => x.UserId == userId);

        if (result == null)
        {
            throw new Exception("User not found");
        }

        return result;
    }

    public void UpdateProfile(UserInfo dto)
    {
        var userInfo = _context.UserInfos.First(x => x.UserId == dto.UserId);

        userInfo.FirstName = dto.FirstName;
        userInfo.LastName = dto.LastName;
        userInfo.DateOfBirth = dto.DateOfBirth;
        userInfo.Description = dto.Description;
        userInfo.PhoneNo = dto.PhoneNo;

        _context.UserInfos.Update(userInfo);
        _context.SaveChanges();
    }
}