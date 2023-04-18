using Backend_BikeApp.Models;
using MySqlConnector;
using Microsoft.AspNetCore.Mvc;

namespace Backend_BikeApp.Services;

public class StationService
{
    static List<string> ReturnStationIds()
    {
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        try
        {
            conn.Open();
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT ID FROM Stations;";
        using var reader = cmd.ExecuteReader();
        var ids = new List<string>();
        while (reader.Read())
        {
            ids.Add(reader.GetString(0));
        }
        return ids;
    }

    static bool ValidateStation(Station record)
    {
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
    public static async Task<ActionResult<IEnumerable<Station>>> GetStationsAsync(
        string page,
        string search,
        string limit
    )
    {
        int totalPages = 0;
        List<Station> stations = new List<Station>();
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();
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

            string query = $"SELECT * FROM Stations";

            if (!string.IsNullOrEmpty(search))
            {
                query =
                    $"SELECT * FROM Stations WHERE NameFIN LIKE '%{search}%' OR NameSWE LIKE '%{search}%' OR NameENG LIKE '%{search}%' OR AddressFIN LIKE '%{search}%' OR AddressSWE LIKE '%{search}%' OR CityFIN LIKE '%{search}%' OR CitySWE LIKE '%{search}%' OR Operator LIKE '%{search}%' OR Capacity LIKE '%{search}%' OR ID LIKE '%{search}%'";
            }

            string totalPageQuery = query.Replace("*", "COUNT(*)");

            query += $" LIMIT {limitNo} OFFSET {pageNo * limitNo}";

            MySqlCommand cmd = new MySqlCommand(query, conn);
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

    public static async Task<ActionResult<Station>> GetStationAsync(int id, string month)
    {
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            // if month is not null, then convert it to number
            // if month is not null, then filter by month
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

            Station station = null!;
            int departureJourneys = 0;
            int returnJourneys = 0;
            float avarageDepartureDistance = 0;
            float avarageReturnDistance = 0;
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

    public static async Task<IActionResult> PutStationAsync(int id, Station station)
    {
        if (id != station.FID)
        {
            return new BadRequestResult();
        }

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

    public static async Task<ActionResult<Station>> PostStationAsync(Station station)
    {
        List<string> ids = ReturnStationIds();
        if (ids.Contains(station.ID!.ToString()))
        {
            return null!;
        }

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
