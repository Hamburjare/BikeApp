using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend_BikeApp.Models;
using Backend_BikeApp.Services;

namespace Backend_BikeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController : ControllerBase {
        // GET: api/Stations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Station>>> GetStationItems()
        {
            // Get the parameters from the query string
            var page = Request.Query["page"];
            try
            {
                var response = await StationService.GetStationsAsync(page);
                if (response != null)
                {
                    return response;
                }
                else
                {
                    return null!;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        // GET: api/Stations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Station>> GetStation(int id)
        {
            try
            {
                var response = await StationService.GetStationAsync(id);
                if (response != null)
                {
                    return response;
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }
    }
}