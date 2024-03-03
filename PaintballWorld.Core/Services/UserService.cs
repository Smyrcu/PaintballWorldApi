using Microsoft.Extensions.Logging;
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
    
    
    
}