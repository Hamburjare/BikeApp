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
        public async Task<ActionResult<IEnumerable<Journey>>> GetJourneyItems(int? page,
            int? limit,
            string? search,
            string? orderBy,
            string? orderDir,
            int? durationMin,
            int? durationMax,
            int? distanceMin,
            int? distanceMax)
        {
            try
            {
                var response = await JourneyService.GetJourneysAsync(
                    page.ToString()!,
                    search!,
                    orderBy!,
                    limit.ToString()!,
                    orderDir!,
                    durationMin.ToString()!,
                    durationMax.ToString()!,
                    distanceMin.ToString()!,
                    distanceMax.ToString()!
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
                var responce = await JourneyService.PutJourneyAsync(id, journey);
                if (responce != null)
                {
                    return responce;
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
                var response = await JourneyService.DeleteJourneyAsync(id);
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

        private bool JourneyExists(int id)
        {
            return true;
        }
    }
}
