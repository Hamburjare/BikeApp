using Backend_BikeApp.Models;
using Backend_BikeApp.Helpers;
using MySqlConnector;
using Microsoft.AspNetCore.Mvc;

namespace Backend_BikeApp.Services;

/// <summary>
/// The class contains functions for interacting with the Journeys table in the MySQL database.
/// </summary>

public class JourneyService
{
    /// <summary>
    /// Validates a journey record.
    /// </summary>
    /// <param name="record">The journey record to validate.</param>
    /// <returns>True if the record is valid, false otherwise.</returns>
    static bool ValidateJourney(Journey record)
    {
        List<string> ids = StationIds.ReturnStationIds();
        
        // Check if the station IDs are valid.
        if (!ids.Contains(record.DepartureStationId!))
        {
            return false;
        }

        if (!ids.Contains(record.ReturnStationId!))
        {
            return false;
        }
        

        /* The above code is performing data validation checks on a record object. It checks if the
        CoveredDistance and Duration properties of the record object can be parsed as integers, and
        if their values are greater than or equal to 10. It also checks if the DepartureTime and
        ReturnTime properties of the record object are not default values and if the ReturnTime is
        greater than or equal to the DepartureTime. Additionally, it checks if the
        DepartureStationId and ReturnStationId properties of the record object are greater than or
        equal to 0. If any of these conditions are not met, the if */
        if (
            !int.TryParse(record.CoveredDistance.ToString(), out int number)
            || !int.TryParse(record.Duration.ToString(), out number)
            || Convert.ToInt32(record.Duration) < 10
            || Convert.ToInt32(record.CoveredDistance) < 10
            || record.DepartureTime == default
            || record.ReturnTime == default
            || record.ReturnTime < record.DepartureTime
            || Convert.ToInt32(record.DepartureStationId) < 0
            || Convert.ToInt32(record.ReturnStationId) < 0
        )
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// It returns a list of journeys, and it's asynchronous
    /// </summary>
    /// <param name="page">The page number to return.</param>
    /// <param name="search">The search string to filter the results.</param>
    /// <param name="orderBy">The column to order the results by.</param>
    /// <param name="limit">The limit of the results to return.</param>
    /// <param name="orderDir">The direction to order the results by.</param>
    /// <param name="durationMin">The minimum duration to filter the results by.</param>
    /// <param name="durationMax">The maximum duration to filter the results by.</param>
    /// <param name="distanceMin">The minimum distance to filter the results by.</param>
    /// <param name="distanceMax">The maximum distance to filter the results by.</param>
    /// <returns>A list of journeys.</returns>
    public static async Task<ActionResult<IEnumerable<Journey>>> GetJourneysAsync(
        string page,
        string search,
        string orderBy,
        string limit,
        string orderDir,
        string durationMin,
        string durationMax,
        string distanceMin,
        string distanceMax
    )
    {
        int totalPages = 0;

        List<Journey> journeys = new List<Journey>();
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();

            // If page is null or not an integer or less than 1, set it to 1
            // This is the default value
            if (page == null || !int.TryParse(page, out int pageNo) || pageNo < 1)
            {
                pageNo = 1;
            }

            // If page is less than 0, set it to 0 after subtracting 1
            pageNo -= 1;
            if (pageNo < 0)
                pageNo = 0;

            // If limit is null or not an integer or less than 1, set it to 10
            // This is the default value
            if (limit == null || !int.TryParse(limit, out int limitNo) || limitNo < 1)
            {
                limitNo = 10;
            }

            string query = $"SELECT * FROM Journeys ";

            // If any of the filters are not empty, add WHERE to the query
            if (
                !string.IsNullOrEmpty(durationMin)
                || !string.IsNullOrEmpty(durationMax)
                || !string.IsNullOrEmpty(distanceMin)
                || !string.IsNullOrEmpty(distanceMax)
                || !string.IsNullOrEmpty(search)
            )
            {
                query += "WHERE ";
            }

            // If durationMin and durationMax are not empty, add BETWEEN to the query
            if (!string.IsNullOrEmpty(durationMin) && !string.IsNullOrEmpty(durationMax))
            {
                query += $"Duration BETWEEN {durationMin} AND {durationMax} ";
            }
            // If durationMin is not empty, add >= to the query
            else if (!string.IsNullOrEmpty(durationMin))
            {
                query += $"Duration >= {durationMin} ";
            }
            // If durationMax is not empty, add <= to the query
            else if (!string.IsNullOrEmpty(durationMax))
            {
                query += $"Duration <= {durationMax} ";
            }

            // If distanceMin and distanceMax are not empty, add BETWEEN to the query
            if (!string.IsNullOrEmpty(distanceMin) && !string.IsNullOrEmpty(distanceMax))
            {
                // If durationMin or durationMax are not empty, add AND to the query
                if (!string.IsNullOrEmpty(durationMin) || !string.IsNullOrEmpty(durationMax))
                {
                    query += "AND ";
                }
                query += $"CoveredDistance BETWEEN {distanceMin} AND {distanceMax} ";
            }
            // If distanceMin is not empty, add >= to the query
            else if (!string.IsNullOrEmpty(distanceMin))
            {
                // If durationMin or durationMax are not empty, add AND to the query
                if (!string.IsNullOrEmpty(durationMin) || !string.IsNullOrEmpty(durationMax))
                {
                    query += "AND ";
                }
                query += $"CoveredDistance >= {distanceMin} ";
            }
            // If distanceMax is not empty, add <= to the query
            else if (!string.IsNullOrEmpty(distanceMax))
            {
                // If durationMin or durationMax are not empty, add AND to the query
                if (!string.IsNullOrEmpty(durationMin) || !string.IsNullOrEmpty(durationMax))
                {
                    query += "AND ";
                }
                query += $"CoveredDistance <= {distanceMax} ";
            }

            // If search is not empty, add LIKE to the query
            if (!string.IsNullOrEmpty(search))
            {
                // If durationMin or durationMax or distanceMin or distanceMax are not empty, add AND to the query
                if (
                    !string.IsNullOrEmpty(durationMin)
                    || !string.IsNullOrEmpty(durationMax)
                    || !string.IsNullOrEmpty(distanceMin)
                    || !string.IsNullOrEmpty(distanceMax)
                )
                {
                    query += "AND ";
                }
                query +=
                    $"(DepartureStationName LIKE '%{search}%' OR ReturnStationName LIKE '%{search}%') ";
            }

            // If orderBy is not empty, add ORDER BY to the query
            if (!string.IsNullOrEmpty(orderBy))
            {
                query += $"ORDER BY {orderBy}";
                // If orderDir is not empty, add ASC or DESC to the query
                if (!string.IsNullOrEmpty(orderDir))
                {
                    // If orderDir is not ASC or DESC, set it to ASC
                    if (orderDir.ToLower() != "asc" && orderDir.ToLower() != "desc")
                    {
                        orderDir = "ASC";
                    }

                    query += $" {orderDir}";
                }
            }

            // Get the total count of journeys
            string totalPageQuery = query.Replace("*", "COUNT(*)");

            // Add LIMIT and OFFSET to the query
            query += $" LIMIT {limitNo} OFFSET {pageNo * limitNo}";

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

            cmd = new MySqlCommand(totalPageQuery, conn);
            using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    // Get the total count of journeys
                    int totalJourneys = reader.GetInt32(0);
                    // Calculate the total number of pages
                    totalPages = totalJourneys / limitNo;
                    // If the total number of journeys is not divisible by the limit, add 1 to the total number of pages
                    if (totalJourneys % limitNo != 0)
                    {
                        totalPages += 1;
                    }
                }
            }

            conn.Close();
        }

        return new OkObjectResult(new { journeys, totalPages });
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
                    return new OkObjectResult(new Journey
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
                    });
                    
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
    /// <returns>OkObjectResult if the journey is updated, BadRequestResult otherwise.</returns>
    public static async Task<IActionResult> PutJourneyAsync(int id, Journey journey)
    {
        // If the id of the journey to update is different from the id of the journey in the body, return a bad request
        if (id != journey.Id)
        {
            return new BadRequestResult();
        }
        
        // If the journey is not valid, return a bad request
        if (ValidateJourney(journey) == false)
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

        return new OkObjectResult(journey);
    }

    /// <summary>
    /// It takes a Journey object as a parameter, and returns a Journey object
    /// </summary>
    /// <param name="journey">The class that represents the data model for the Journey table in the
    /// database.</param>
    /// <returns>A Journey object if the journey is valid, null otherwise.</returns>
    public static async Task<ActionResult<Journey>> PostJourneyAsync(Journey journey)
    {
        // Check if the journey is valid
        if (ValidateJourney(journey) == false)
            return null!;

        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();

            // Get the last id from the database
            MySqlCommand lastId = new MySqlCommand(
                "SELECT Id FROM Journeys ORDER BY Id DESC LIMIT 1",
                conn
            );

            // If there is a last id, increment it by 1, otherwise set it to 1
            using (MySqlDataReader reader = await lastId.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    journey.Id = reader.GetInt32("Id") + 1;
                }
            }
            
            // Insert the journey into the database
            MySqlCommand cmd = new MySqlCommand(
                "INSERT INTO Journeys (Id, DepartureTime, ReturnTime, DepartureStationId, DepartureStationName, ReturnStationId, ReturnStationName, CoveredDistance, Duration) VALUES (@id, @departureTime, @returnTime, @departureStationId, @departureStationName, @returnStationId, @returnStationName, @coveredDistance, @duration)",
                conn
            );
            cmd.Parameters.AddWithValue("@id", journey.Id);
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
    /// <returns>OkResult</returns>
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
