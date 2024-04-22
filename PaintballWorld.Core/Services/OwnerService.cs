using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Core.Models;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly ApplicationDbContext _context;

        public OwnerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Approve()
        {
            throw new NotImplementedException();
        }

        public Guid? GetFieldId(string userId)
        {
            var owner = _context.Owners.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
            if (owner == null)
            {
                return null;
            }

            var field = _context.Fields.FirstOrDefault(x => x.OwnerId == owner.Id);

            return field?.Id.Value;
        }
    }
}
