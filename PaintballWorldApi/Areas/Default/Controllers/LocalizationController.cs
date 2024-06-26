﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.Areas.Default.Models;
using PaintballWorld.Core.Interfaces;

namespace PaintballWorld.API.Areas.Default.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Default")]
    [AllowAnonymous]
    public class LocalizationController : Controller
    {
        private readonly IAutocompleteService _autocompleteService;
        private readonly ILogger<LocalizationController> _logger;


        public LocalizationController(IAutocompleteService autocompleteService, ILogger<LocalizationController> logger)
        {
            _autocompleteService = autocompleteService;
            _logger = logger;
        }

        /// <summary>
        /// Autocomplete
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{city}")]
        public IActionResult CityAutocomplete([FromRoute] string city)
        {
            var result = _autocompleteService.GetCityAutocomplete(city);

            return Ok(new AutocompleteResponse
            {
                IsSuccess = true,
                Errors = [],
                Message = "",
                Data = result
            });

        }



    }
}
