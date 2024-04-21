using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaintballWorld.Core.Models;

namespace PaintballWorld.Core.Interfaces
{
    public interface IReservationService
    {
        Task Create(EventModel model);
    }
}
