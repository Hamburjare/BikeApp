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
            var search = Request.Query["search"];
            try
            {
                var response = await StationService.GetStationsAsync(page!, search!);
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
                return BadRequest(ex.Message);
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
                return BadRequest(ex.Message);
            }
        }


        // PUT: api/Stations
        [HttpPut]
        public async Task<IActionResult> PutStation(int id, Station station)
        {
            if (id != station.FID)
            {
                return BadRequest("Id mismatch");
            }
            try {
                await StationService.PutStationAsync(id, station);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Stations
        [HttpPost]
        public async Task<ActionResult<Station>> PostStation(Station station)
        {
            try
            {
                var response = await StationService.PostStationAsync(station);
                if (response != null)
                {
                    return CreatedAtAction("GetStation", new { id = station.FID }, station);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Stations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Station>> DeleteStation(int id)
        {
            try
            {
                await StationService.DeleteStationAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        
    }
}