using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectingApp.API.Data;
using ConnectingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ConnectingApp.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly DataContext _ctx;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
                                         DataContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        // get all values
        [HttpGet]
        public async Task<IActionResult> GetValues()
        {
            return Ok(await _ctx.Values.ToListAsync());
        }

        // get specific value
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            var value = await _ctx.Values.FirstOrDefaultAsync(v => v.Id == id);
            if (value == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(value);
            }
        }
    }
}
