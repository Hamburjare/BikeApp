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
    public class JourneysController : ControllerBase
    {
        // GET: api/Journeys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Journey>>> GetJourneyItems()
        {
            // Get the parameters from the query string
            var page = Request.Query["page"];
            var limit = Request.Query["limit"];
            var search = Request.Query["search"];
            var orderBy = Request.Query["orderBy"];
            var orderDir = Request.Query["orderDir"];
            var durationMin = Request.Query["durationMin"];
            var durationMax = Request.Query["durationMax"];
            var distanceMin = Request.Query["distanceMin"];
            var distanceMax = Request.Query["distanceMax"];

            try
            {
                var response = await JourneyService.GetJourneysAsync(
                    page!,
                    search!,
                    orderBy!,
                    limit!,
                    orderDir!,
                    durationMin!,
                    durationMax!,
                    distanceMin!,
                    distanceMax!
                );
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
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Journeys/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJourneyAsync(int id, Journey journey)
        {
            if (id != journey.Id)
            {
                return BadRequest("Id mismatch");
            }
            try
            {
                await JourneyService.PutJourneyAsync(id, journey);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Journeys
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Journey>> PostJourney(Journey journey)
        {
            try
            {
                var response = await JourneyService.PostJourneyAsync(journey);
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

        // DELETE: api/Journeys/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJourney(int id)
        {
            try
            {
                await JourneyService.DeleteJourneyAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private bool JourneyExists(int id)
        {
            return true;
        }
    }
}
