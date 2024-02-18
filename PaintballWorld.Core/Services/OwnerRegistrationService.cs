using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Services
{
    public class OwnerRegistrationService : IOwnerRegistrationService
    {
        private readonly ApplicationDbContext _context;

        public OwnerRegistrationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void RegisterOwner(Owner owner)
        {
            _context.Owners.Add(owner);
            _context.SaveChanges();
        }
    }
}
