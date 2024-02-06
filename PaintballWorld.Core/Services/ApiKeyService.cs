using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Logging;
using Microsoft.Extensions.Logging;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Infrastructure;

namespace PaintballWorld.Core.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly ILogger<ApiKeyService> _logger;
        private readonly ApplicationDbContext _context;

        public ApiKeyService(ILogger<ApiKeyService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public bool IsApiKeyValid(string apiKey)
        {
            return _context.ApiKeys.Any(x => x.Key.Equals(apiKey));
        }
    }
}
