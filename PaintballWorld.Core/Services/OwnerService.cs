using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Core.Models;

namespace PaintballWorld.Core.Services
{
    public class OwnerService : IOwnerService
    {
        public void RegisterOwner(IdentityUser user, OwnerModel request)
        {
            throw new NotImplementedException();
        }
    }
}
