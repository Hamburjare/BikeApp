using Backend_BikeApp.Models;
using MySqlConnector;
using Microsoft.AspNetCore.Mvc;

namespace Backend_BikeApp.Services;

public class JourneyService
{
    /// <summary>
    /// It returns a list of journeys, and it's asynchronous
    /// </summary>
    /// <param name="page">The page number to return.</param>
    public static async Task<ActionResult<IEnumerable<Journey>>> GetJourneysAsync(
        string page,
        string search,
        string orderBy


    )
    {
        List<Journey> journeys = new List<Journey>();
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();
            if (page == null || !int.TryParse(page, out int pageNo) || pageNo < 0)
            {
                pageNo = 0;
            }

            string query = $"SELECT * FROM Journeys ";

            if (!string.IsNullOrEmpty(search))
            {
                query =
                    $"SELECT * FROM Journeys WHERE DepartureStationName LIKE '%{search}%' OR ReturnStationName LIKE '%{search}%' OR DepartureTime LIKE '%{search}%' OR ReturnTime LIKE '%{search}%' OR CoveredDistance LIKE '%{search}%' OR Duration LIKE '%{search}%' OR Id LIKE '%{search}%' OR DepartureStationId LIKE '%{search}%' OR ReturnStationId LIKE '%{search}%'";
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                query += $"ORDER BY {orderBy}";
            }

            query += $" LIMIT 10 OFFSET {pageNo * 10}";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Journey journey = new Journey
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
                    journeys.Add(journey);
                }
            }
        }

        if (journeys.Count == 0)
            return new NotFoundResult();

        return journeys;
    }

    /// <summary>
    /// It returns a journey by id, and it's asynchronous
    /// </summary>
    /// <param name="id">The id of the journey to return.</param>

    public static async Task<ActionResult<Journey>> GetJourneyAsync(int id)
    {
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Journeys WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
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
            }
            conn.Close();
        }
        return null!;
    }

    /// <summary>
    /// It updates a journey by id, and it's asynchronous
    /// </summary>
    /// <param name="id">The id of the journey to update.</param>
    /// <param name="journey">The journey to update.</param>
    public static async Task<IActionResult> PutJourneyAsync(int id, Journey journey)
    {
        if (id != journey.Id)
        {
            return new BadRequestResult();
        }
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(
                "UPDATE Journeys SET DepartureTime = @departureTime, ReturnTime = @returnTime, DepartureStationId = @departureStationId, DepartureStationName = @departureStationName, ReturnStationId = @returnStationId, ReturnStationName = @returnStationName, CoveredDistance = @coveredDistance, Duration = @duration WHERE Id = @id",
                conn
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
            conn.Close();
        }

        return new OkResult();
    }

    /// <summary>
    /// It takes a Journey object as a parameter, and returns a Journey object
    /// </summary>
    /// <param name="Journey">The class that represents the data model for the Journey table in the
    /// database.</param>
    public static async Task<ActionResult<Journey>> PostJourneyAsync(Journey journey)
    {
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(
                "INSERT INTO Journeys (DepartureTime, ReturnTime, DepartureStationId, DepartureStationName, ReturnStationId, ReturnStationName, CoveredDistance, Duration) VALUES (@departureTime, @returnTime, @departureStationId, @departureStationName, @returnStationId, @returnStationName, @coveredDistance, @duration)",
                conn
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
            conn.Close();
        }

        return new CreatedAtActionResult(
            "GetJourney",
            "Journeys",
            new { id = journey.Id },
            journey
        );
    }

    /// <summary>
    /// It deletes a journey by id, and it's asynchronous
    /// </summary>
    /// <param name="id">The id of the journey to delete.</param>
    public static async Task<IActionResult> DeleteJourneyAsync(int id)
    {
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM Journeys WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
            conn.Close();
        }
        return new OkResult();
    }
}
