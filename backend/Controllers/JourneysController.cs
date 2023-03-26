using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend_BikeApp.Models;
using Backend_BikeApp.Services;
using MySqlConnector;

namespace Backend_BikeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JourneysController : ControllerBase
    {
        // GET: api/Journeys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Journey>>> GetJourneyItems()
        {
            // Get the parameters from the query string
            var page = Request.Query["page"];
            try
            {
                var response = await JourneyService.GetJourneysAsync(page);
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

        // GET: api/Journeys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Journey>> GetJourney(int id)
        {
            try
            {
                var response = await JourneyService.GetJourneyAsync(id);
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

        // PUT: api/Journeys/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJourney(int id, Journey journey)
        {
            if (id != journey.Id)
            {
                return BadRequest();
            }
            try
            {
                await JourneyService.PutJourneyAsync(id, journey);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        // POST: api/Journeys
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Journey>> PostJourney(Journey journey)
        {
            try
            {
                await JourneyService.PostJourneyAsync(journey);
                return CreatedAtAction("GetJourney", new { id = journey.Id }, journey);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        // DELETE: api/Journeys/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJourney(int id)
        {
            try {
                await JourneyService.DeleteJourneyAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        private bool JourneyExists(int id)
        {
            return true;
        }
    }
}
