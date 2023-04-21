using Backend_BikeApp.Models;
using Backend_BikeApp.Helpers;
using MySqlConnector;
using Microsoft.AspNetCore.Mvc;

namespace Backend_BikeApp.Services;

/// <summary>
/// The class contains functions for interacting with the Stations table in the MySQL database.
/// </summary>

public class StationService
{
    

    /// <summary>
    /// The function takes a Station record as input and returns a boolean value indicating whether the
    /// record is valid or not.
    /// </summary>
    /// <param name="record">A Station record.</param>
    /// <returns>A boolean value indicating whether the record is valid or not.</returns>
    static bool ValidateStation(Station record)
    {
        /* The above code is performing data validation checks on a record object. It checks if the
        record's FID, Capacity, Longitude, and Latitude properties can be parsed as integers or
        doubles, and if they fall within certain ranges. It also checks if the record's ID, NameFIN,
        NameENG, NameSWE, AddressFIN, AddressSWE, CityFIN, CitySWE, and Operator properties are not
        null or empty. If any of these checks fail, the if statement will evaluate to true. */
        if (
            !int.TryParse(record.FID.ToString(), out int number)
            || !int.TryParse(record.Capacity.ToString(), out number)
            || !double.TryParse(record.Longitude, out double number1)
            || !double.TryParse(record.Latitude, out number1)
            || Convert.ToDouble(record.Longitude) < -180
            || Convert.ToDouble(record.Longitude) > 180
            || Convert.ToDouble(record.Latitude) < -90
            || Convert.ToDouble(record.Latitude) > 90
            || Convert.ToInt32(record.Capacity) < 0
            || Convert.ToInt32(record.ID) < 0
            || record.NameFIN == null
            || string.IsNullOrWhiteSpace(record.NameFIN)
            || record.NameENG == null
            || string.IsNullOrWhiteSpace(record.NameENG)
            || record.NameSWE == null
            || string.IsNullOrWhiteSpace(record.NameSWE)
            || record.AddressFIN == null
            || string.IsNullOrWhiteSpace(record.AddressFIN)
            || record.AddressSWE == null
            || string.IsNullOrWhiteSpace(record.AddressSWE)
            || record.CityFIN == null
            || string.IsNullOrWhiteSpace(record.CityFIN)
            || record.CitySWE == null
            || string.IsNullOrWhiteSpace(record.CitySWE)
            || record.Operator == null
            || string.IsNullOrWhiteSpace(record.Operator)
        )
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// It returns a list of stations, and it's asynchronous
    /// </summary>
    /// <param name="page">The page number to return.</param>
    /// <param name="search">The search string to filter the results.</param>
    /// <param name="limit">The limit of the results to return.</param>
    /// <returns>A list of stations.</returns>
    public static async Task<ActionResult<IEnumerable<Station>>> GetStationsAsync(
        string page,
        string search,
        string limit
    )
    {
        // Storing the total number of pages in the database.
        int totalPages = 0;

        // Creating a new List object to store the stations.
        List<Station> stations = new List<Station>();

        // Connecting to the database.
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            // tries to open the connection to the database.
            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            // If page, search or limit is null, it will be set to default values.
            if (page == null || !int.TryParse(page, out int pageNo) || pageNo < 1)
            {
                pageNo = 1;
            }

            pageNo -= 1;
            if (pageNo < 0)
                pageNo = 0;


            if (limit == null || !int.TryParse(limit, out int limitNo) || limitNo < 1)
            {
                limitNo = 10;
            }

            /* Query for getting data from database */
            string query = $"SELECT * FROM Stations";
            
            if (!string.IsNullOrEmpty(search))
            {
                query =
                    $"SELECT * FROM Stations WHERE NameFIN LIKE '%{search}%' OR NameSWE LIKE '%{search}%' OR NameENG LIKE '%{search}%' OR AddressFIN LIKE '%{search}%' OR AddressSWE LIKE '%{search}%' OR CityFIN LIKE '%{search}%' OR CitySWE LIKE '%{search}%' OR Operator LIKE '%{search}%' OR Capacity LIKE '%{search}%' OR ID LIKE '%{search}%'";
            }

            /* Query for getting the total number of pages in the database. */
            string totalPageQuery = query.Replace("*", "COUNT(*)");

            /* Adding the LIMIT and OFFSET clauses to the query string. */
            query += $" LIMIT {limitNo} OFFSET {pageNo * limitNo}";

            MySqlCommand cmd = new MySqlCommand(query, conn);

            // Executing the query and storing the result set.
            using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Station station = new Station
                    {
                        FID = reader.GetInt32("FID"),
                        ID = reader.GetString("ID"),
                        NameFIN = reader.GetString("NameFIN"),
                        NameSWE = reader.GetString("NameSWE"),
                        NameENG = reader.GetString("NameENG"),
                        AddressFIN = reader.GetString("AddressFIN"),
                        AddressSWE = reader.GetString("AddressSWE"),
                        CityFIN = reader.GetString("CityFIN"),
                        CitySWE = reader.GetString("CitySWE"),
                        Operator = reader.GetString("Operator"),
                        Capacity = reader.GetInt32("Capacity"),
                        Longitude = reader.GetString("Longitude"),
                        Latitude = reader.GetString("Latitude")
                    };
                    stations.Add(station);
                }
            }

            // Getting the total number of pages in the database. Using given limit and offset. 
            cmd = new MySqlCommand(totalPageQuery, conn);
            using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    int totalStations = reader.GetInt32(0);
                    totalPages = totalStations / limitNo;
                    if (totalStations % limitNo != 0)
                    {
                        totalPages += 1;
                    }
                }
            }
            conn.Close();
        }

        return new OkObjectResult(new { stations, totalPages });
    }

    /// <summary>
    /// It returns a station by id, and it's asynchronous
    /// </summary>
    /// <param name="id">The id of the station to return.</param>
    /// <param name="month">The month to filter stats by</param>
    /// <returns>A station with the given id.</returns>

    public static async Task<ActionResult<Station>> GetStationAsync(int id, string month)
    {
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            // If month is not null, it will be converted to a number.
            if (month != null)
            {
                string[] months = new string[]
                {
                    "january",
                    "february",
                    "march",
                    "april",
                    "may",
                    "june",
                    "july",
                    "august",
                    "september",
                    "october",
                    "november",
                    "december"
                };
                for (int i = 0; i < months.Length; i++)
                {
                    if (months[i] == month.ToLower())
                    {
                        month = (i + 1).ToString();
                    }
                }
            }

            // New station object
            Station station = null!;

            // Variables for storing the number of departures and returns from the station
            int departureJourneys = 0;
            int returnJourneys = 0;

            // Variables for storing the average distance of departures and returns from the station
            float avarageDepartureDistance = 0;
            float avarageReturnDistance = 0;
            
            // Variables for storing the top 5 departure and return stations from the station
            List<string> top5DepartureStations = new List<string>();
            List<string> top5ReturnStations = new List<string>();

            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Stations WHERE ID = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    station = new Station
                    {
                        FID = reader.GetInt32("FID"),
                        ID = reader.GetString("ID"),
                        NameFIN = reader.GetString("NameFIN"),
                        NameSWE = reader.GetString("NameSWE"),
                        NameENG = reader.GetString("NameENG"),
                        AddressFIN = reader.GetString("AddressFIN"),
                        AddressSWE = reader.GetString("AddressSWE"),
                        CityFIN = reader.GetString("CityFIN"),
                        CitySWE = reader.GetString("CitySWE"),
                        Operator = reader.GetString("Operator"),
                        Capacity = reader.GetInt32("Capacity"),
                        Longitude = reader.GetString("Longitude"),
                        Latitude = reader.GetString("Latitude")
                    };
                }
            }

            // if station is null, then return not found
            if (station == null)
                return new NotFoundResult();

            // get number of journeys from this station
            string query = "SELECT COUNT(*) FROM Journeys WHERE DepartureStationId = @id";

            // if month is not null, then filter by month
            if (month != null)
            {
                query += " AND MONTH(DepartureTime) = @month AND MONTH(ReturnTime) = @month";
            }

            cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@month", month);
            using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    if (reader[0] == DBNull.Value)
                        departureJourneys = 0;
                    else
                        departureJourneys = reader.GetInt32(0);
                }
            }

            // Get number of journeys to this station
            query = "SELECT COUNT(*) FROM Journeys WHERE ReturnStationId = @id";

            // if month is not null, then filter by month
            if (month != null)
            {
                query += " AND MONTH(DepartureTime) = @month AND MONTH(ReturnTime) = @month";
            }

            cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@month", month);
            using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    if (reader[0] == DBNull.Value)
                        returnJourneys = 0;
                    else
                        returnJourneys = reader.GetInt32(0);
                }
            }

            // Get avarage distance of journeys from this station
            query = "SELECT AVG(CoveredDistance) FROM Journeys WHERE DepartureStationId = @id";

            // if month is not null, then filter by month
            if (month != null)
            {
                query += " AND MONTH(DepartureTime) = @month AND MONTH(ReturnTime) = @month";
            }

            cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@month", month);
            using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    if (reader[0] == DBNull.Value)
                        avarageDepartureDistance = 0;
                    else
                        avarageDepartureDistance = reader.GetFloat(0);
                }
            }

            // Get avarage distance of journeys to this station
            query = "SELECT AVG(CoveredDistance) FROM Journeys WHERE ReturnStationId = @id";

            // if month is not null, then filter by month
            if (month != null)
            {
                query += " AND MONTH(DepartureTime) = @month AND MONTH(ReturnTime) = @month";
            }

            cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@month", month);
            using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    if (reader[0] == DBNull.Value)
                        avarageReturnDistance = 0;
                    else
                        avarageReturnDistance = reader.GetFloat(0);
                }
            }

            // Get Top 5 most popular departure stations for journeys ending at the station
            query =
                "SELECT Stations.NameFIN, COUNT(*) AS count FROM Journeys INNER JOIN Stations ON Journeys.DepartureStationId = Stations.ID WHERE Journeys.ReturnStationId = @id";

            // if month is not null, then filter by month
            if (month != null)
            {
                query += " AND MONTH(DepartureTime) = @month AND MONTH(ReturnTime) = @month";
            }

            query += " GROUP BY Journeys.DepartureStationId ORDER BY count DESC LIMIT 5";

            cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@month", month);
            using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (reader[0] == DBNull.Value)
                        continue;
                    top5DepartureStations.Add(reader.GetString(0));
                }
            }

            // Get Top 5 most popular return stations for journeys starting from the station
            query =
                "SELECT Stations.NameFIN, COUNT(*) AS count FROM Journeys INNER JOIN Stations ON Journeys.ReturnStationId = Stations.ID WHERE Journeys.DepartureStationId = @id";

            // if month is not null, then filter by month
            if (month != null)
            {
                query += " AND MONTH(DepartureTime) = @month AND MONTH(ReturnTime) = @month";
            }

            query += " GROUP BY Journeys.ReturnStationId ORDER BY count DESC LIMIT 5";

            cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@month", month);
            using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (reader[0] == DBNull.Value)
                        continue;
                    top5ReturnStations.Add(reader.GetString(0));
                }
            }

            conn.Close();

            return new OkObjectResult(
                new
                {
                    station,
                    departureJourneys,
                    returnJourneys,
                    avarageDepartureDistance,
                    avarageReturnDistance,
                    top5DepartureStations,
                    top5ReturnStations
                }
            );
        }
    }

    /// <summary>
    /// Updates a station with the given ID and information.
    /// </summary>
    /// <param name="id">The ID of the station to update.</param>
    /// <param name="station">The station object containing the new information.</param>
    public static async Task<IActionResult> PutStationAsync(int id, Station station)
    {
        // Check that the given id matches the id in the station object
        if (id != station.FID)
        {
            return new BadRequestResult();
        }
        
        // Check that the station object is valid
        if (ValidateStation(station) == false)
        {
            return new BadRequestResult();
        }

        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(
                "UPDATE Stations SET ID = @ID, NameFIN = @NameFIN, NameSWE = @NameSWE, NameENG = @NameENG, AddressFIN = @AddressFIN, AddressSWE = @AddressSWE, CityFIN = @CityFIN, CitySWE = @CitySWE, Operator = @Operator, Capacity = @Capacity, Longitude = @Longitude, Latitude = @Latitude WHERE FID = @FID",
                conn
            );
            cmd.Parameters.AddWithValue("@FID", station.FID);
            cmd.Parameters.AddWithValue("@ID", station.ID);
            cmd.Parameters.AddWithValue("@NameFIN", station.NameFIN);
            cmd.Parameters.AddWithValue("@NameSWE", station.NameSWE);
            cmd.Parameters.AddWithValue("@NameENG", station.NameENG);
            cmd.Parameters.AddWithValue("@AddressFIN", station.AddressFIN);
            cmd.Parameters.AddWithValue("@AddressSWE", station.AddressSWE);
            cmd.Parameters.AddWithValue("@CityFIN", station.CityFIN);
            cmd.Parameters.AddWithValue("@CitySWE", station.CitySWE);
            cmd.Parameters.AddWithValue("@Operator", station.Operator);
            cmd.Parameters.AddWithValue("@Capacity", station.Capacity);
            cmd.Parameters.AddWithValue("@Longitude", station.Longitude);
            cmd.Parameters.AddWithValue("@Latitude", station.Latitude);
            await cmd.ExecuteNonQueryAsync();
            conn.Close();
        }
        return new OkObjectResult(station);
    }

    /// <summary>
    /// Posts a new station to the database asynchronously.
    /// </summary>
    /// <param name="station">The station object to post.</param>
    public static async Task<ActionResult<Station>> PostStationAsync(Station station)
    {
        // Checks if the station already exists
        List<string> ids = StationIds.ReturnStationIds();
        if (ids.Contains(station.ID!.ToString()))
        {
            return null!;
        }

        // Check that the station object is valid
        if (ValidateStation(station) == false)
            return null!;

        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();

            MySqlCommand lastId = new MySqlCommand(
                "SELECT FID FROM Stations ORDER BY FID DESC LIMIT 1",
                conn
            );

            using (MySqlDataReader reader = await lastId.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    station.FID = reader.GetInt32("FID") + 1;
                }
            }
            MySqlCommand cmd = new MySqlCommand(
                "INSERT INTO Stations (FID, ID, NameFIN, NameSWE, NameENG, AddressFIN, AddressSWE, CityFIN, CitySWE, Operator, Capacity, Longitude, Latitude) VALUES (@FID, @ID, @NameFIN, @NameSWE, @NameENG, @AddressFIN, @AddressSWE, @CityFIN, @CitySWE, @Operator, @Capacity, @Longitude, @Latitude)",
                conn
            );
            cmd.Parameters.AddWithValue("@FID", station.FID);
            cmd.Parameters.AddWithValue("@ID", station.ID);
            cmd.Parameters.AddWithValue("@NameFIN", station.NameFIN);
            cmd.Parameters.AddWithValue("@NameSWE", station.NameSWE);
            cmd.Parameters.AddWithValue("@NameENG", station.NameENG);
            cmd.Parameters.AddWithValue("@AddressFIN", station.AddressFIN);
            cmd.Parameters.AddWithValue("@AddressSWE", station.AddressSWE);
            cmd.Parameters.AddWithValue("@CityFIN", station.CityFIN);
            cmd.Parameters.AddWithValue("@CitySWE", station.CitySWE);
            cmd.Parameters.AddWithValue("@Operator", station.Operator);
            cmd.Parameters.AddWithValue("@Capacity", station.Capacity);
            cmd.Parameters.AddWithValue("@Longitude", station.Longitude);
            cmd.Parameters.AddWithValue("@Latitude", station.Latitude);
            await cmd.ExecuteNonQueryAsync();
            conn.Close();
        }
        return new CreatedAtActionResult(
            "GetStation",
            "Stations",
            new { id = station.FID },
            station
        );
    }

    /// <summary>
    /// Deletes a station with the given ID from the database.
    /// </summary>
    /// <param name="id">The ID of the station to delete.</param>
    public static async Task<IActionResult> DeleteStationAsync(int id)
    {
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM Stations WHERE FID = @FID", conn);
            cmd.Parameters.AddWithValue("@FID", id);
            await cmd.ExecuteNonQueryAsync();
            conn.Close();
        }
        return new OkResult();
    }
}
