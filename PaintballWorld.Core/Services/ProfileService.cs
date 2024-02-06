using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProfileService> _logger;

        public ProfileService(ApplicationDbContext context, ILogger<ProfileService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void FinishRegistration(IdentityUser user, DateTime dateOfBirth)
        {
            var userInfo = new UserInfo
            {
                DateOfBirth = dateOfBirth,
                UserId = Guid.Parse(user.Id)
            };

            _context.UserInfos.Add(userInfo);
            _context.SaveChanges();


        }
    }
}
