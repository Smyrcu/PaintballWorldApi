using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Infrastructure;

namespace PaintballWorld.Core.Services
{
    public class AutocompleteService : IAutocompleteService
    {
        private readonly ApplicationDbContext _context;

        public AutocompleteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<string?> GetCityAutocomplete(string cityName)
        {
            var cities = _context.OsmCities.Where(x => EF.Functions.Like(x.Name, $"{cityName}%")).OrderBy(x => x.Name).Take(5)
                .Select(x => x.Name).AsEnumerable();

            return cities;

        }



    }
}
