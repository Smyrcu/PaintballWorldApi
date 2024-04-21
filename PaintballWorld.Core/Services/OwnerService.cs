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

        public FieldId GetFieldId(OwnerId owner)
        {
            return _context.Fields.First(x => x.OwnerId == owner).Id;
        }
    }
}
