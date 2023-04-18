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

        /// <summary>
        /// Get all stations
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="limit">Number of items per page</param>
        /// <param name="search">Search string</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Station>>> GetStationItems(
            int? page,
            int? limit,
            string? search
        )
        {
            try
            {
                var response = await StationService.GetStationsAsync(page.ToString()!, search!, limit.ToString()!);
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
        public async Task<ActionResult<Station>> GetStation(int id, string? month)
        {
            try
            {
                var response = await StationService.GetStationAsync(id, month!);
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
                var response = await StationService.PutStationAsync(id, station);
                if (response != null)
                {
                    return response;
                }
                else
                {
                    return BadRequest();
                }
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
                    return BadRequest();
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
        public async Task<IActionResult> DeleteStation(int id)
        {
            try
            {
                var response = await StationService.DeleteStationAsync(id);
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
        
    }
}