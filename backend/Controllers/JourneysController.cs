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
            return await JourneyService.GetJourneysAsync(page);
        }

        // GET: api/Journeys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Journey>> GetJourney(int id)
        {
            await using var connection = new MySqlConnection(MySQLHelper.connectionString);
            await connection.OpenAsync();
            using var cmd = new MySqlCommand("SELECT * FROM Journeys WHERE Id = @id", connection);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Journey
                {
                    Id = reader.GetInt32("Id"),
                    DepartureTime = reader.GetDateTime("DepartureTime"),
                    ReturnTime = reader.GetDateTime("ReturnTime"),
                    DepartureStationId = reader.GetString("DepartureStationId"),
                    DepartureStationName = reader.GetString("DepartureStationName"),
                    ReturnStationId = reader.GetString("ReturnStationId"),
                    ReturnStationName = reader.GetString("ReturnStationName"),
                    CoveredDistance = reader.GetInt32("CoveredDistance"),
                    Duration = reader.GetInt32("Duration")
                };
            }
            return NotFound();
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
            await using var connection = new MySqlConnection(MySQLHelper.connectionString);
            await connection.OpenAsync();
            using var cmd = new MySqlCommand(
                "UPDATE Journeys SET DepartureTime = @departureTime, ReturnTime = @returnTime, DepartureStationId = @departureStationId, DepartureStationName = @departureStationName, ReturnStationId = @returnStationId, ReturnStationName = @returnStationName, CoveredDistance = @coveredDistance, Duration = @duration WHERE Id = @id",
                connection
            );
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@departureTime", journey.DepartureTime);
            cmd.Parameters.AddWithValue("@returnTime", journey.ReturnTime);
            cmd.Parameters.AddWithValue("@departureStationId", journey.DepartureStationId);
            cmd.Parameters.AddWithValue("@departureStationName", journey.DepartureStationName);
            cmd.Parameters.AddWithValue("@returnStationId", journey.ReturnStationId);
            cmd.Parameters.AddWithValue("@returnStationName", journey.ReturnStationName);
            cmd.Parameters.AddWithValue("@coveredDistance", journey.CoveredDistance);
            cmd.Parameters.AddWithValue("@duration", journey.Duration);
            await cmd.ExecuteNonQueryAsync();
            return NoContent();
        }

        // POST: api/Journeys
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Journey>> PostJourney(Journey journey)
        {
            await using var connection = new MySqlConnection(MySQLHelper.connectionString);
            await connection.OpenAsync();
            using var cmd = new MySqlCommand(
                "INSERT INTO Journeys (DepartureTime, ReturnTime, DepartureStationId, DepartureStationName, ReturnStationId, ReturnStationName, CoveredDistance, Duration) VALUES (@departureTime, @returnTime, @departureStationId, @departureStationName, @returnStationId, @returnStationName, @coveredDistance, @duration)",
                connection
            );
            cmd.Parameters.AddWithValue("@departureTime", journey.DepartureTime);
            cmd.Parameters.AddWithValue("@returnTime", journey.ReturnTime);
            cmd.Parameters.AddWithValue("@departureStationId", journey.DepartureStationId);
            cmd.Parameters.AddWithValue("@departureStationName", journey.DepartureStationName);
            cmd.Parameters.AddWithValue("@returnStationId", journey.ReturnStationId);
            cmd.Parameters.AddWithValue("@returnStationName", journey.ReturnStationName);
            cmd.Parameters.AddWithValue("@coveredDistance", journey.CoveredDistance);
            cmd.Parameters.AddWithValue("@duration", journey.Duration);
            await cmd.ExecuteNonQueryAsync();
            return CreatedAtAction("GetJourney", new { id = journey.Id }, journey);
        }

        // DELETE: api/Journeys/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJourney(int id)
        {
            await using var connection = new MySqlConnection(MySQLHelper.connectionString);
            await connection.OpenAsync();
            using var cmd = new MySqlCommand("DELETE FROM Journeys WHERE Id = @id", connection);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
            return NoContent();
        }

        private bool JourneyExists(int id)
        {
            return true;
        }
    }
}
