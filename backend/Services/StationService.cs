using Backend_BikeApp.Models;
using MySqlConnector;
using Microsoft.AspNetCore.Mvc;

namespace Backend_BikeApp.Services;

public class StationService
{
    /// <summary>
    /// It returns a list of stations, and it's asynchronous
    /// </summary>
    /// <param name="page">The page number to return.</param>
    public static async Task<ActionResult<IEnumerable<Station>>> GetStationsAsync(string page)
    {
        List<Station> stations = new List<Station>();
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();
            if (page == null || !int.TryParse(page, out int pageNo) || pageNo < 0)
            {
                pageNo = 0;
            }

            MySqlCommand cmd = new MySqlCommand(
                $"SELECT * FROM Stations LIMIT 10 OFFSET {pageNo * 10}",
                conn
            );
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
        }
        return stations;
    }

    /// <summary>
    /// It returns a station by id, and it's asynchronous
    /// </summary>
    /// <param name="id">The id of the station to return.</param>

    public static async Task<ActionResult<Station>> GetStationAsync(int id)
    {
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Stations WHERE ID = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new Station
                    {
                        FID = reader.GetInt32("FID"),
                        ID = reader.GetString("ID"),
                        NameFIN = reader.GetString("NameFIN"),
                        NameSWE = reader.GetString("NameSWE"),
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
        }
        return null!;
    }

    /// <summary>
    /// It returns a list of stations by city, and it's asynchronous
    /// </summary>
    /// <param name="city">The city of the stations to return.</param>

    public static async Task<ActionResult<IEnumerable<Station>>> GetStationsByCityAsync(string city)
    {
        List<Station> stations = new List<Station>();
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(
                "SELECT * FROM Stations WHERE CityFIN = @city",
                conn
            );
            cmd.Parameters.AddWithValue("@city", city);
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
        }
        return stations;
    }

    /// <summary>
    /// It returns a list of stations by operator, and it's asynchronous
    /// </summary>
    /// <param name="operator">The operator of the stations to return.</param>

    public static async Task<ActionResult<IEnumerable<Station>>> GetStationsByOperatorAsync(
        string @operator
    )
    {
        List<Station> stations = new List<Station>();
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(
                "SELECT * FROM Stations WHERE Operator = @operator",
                conn
            );
            cmd.Parameters.AddWithValue("@operator", @operator);
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
        }
        return stations;
    }

    /// <summary>
    /// It returns a list of stations by capacity, and it's asynchronous
    /// </summary>
    /// <param name="capacity">The capacity of the stations to return.</param>

    public static async Task<ActionResult<IEnumerable<Station>>> GetStationsByCapacityAsync(
        string capacity
    )
    {
        List<Station> stations = new List<Station>();
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(
                "SELECT * FROM Stations WHERE Capacity = @capacity",
                conn
            );
            cmd.Parameters.AddWithValue("@capacity", capacity);
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
        }
        return stations;
    }

    /// <summary>
    /// It returns a list of stations by location, and it's asynchronous
    /// </summary>
    /// <param name="locationX">The locationX of the stations to return.</param>
    /// <param name="locationY">The locationY of the stations to return.</param>

    public static async Task<ActionResult<IEnumerable<Station>>> GetStationsByLocationAsync(
        string locationX,
        string locationY
    )
    {
        List<Station> stations = new List<Station>();
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(
                "SELECT * FROM Stations WHERE LocationX = @locationX AND LocationY = @locationY",
                conn
            );
            cmd.Parameters.AddWithValue("@locationX", locationX);
            cmd.Parameters.AddWithValue("@locationY", locationY);
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
        }
        return stations;
    }

    /// <summary>
    /// It returns a list of stations by name, and it's asynchronous
    /// </summary>
    /// <param name="name">The name of the stations to return.</param>

    public static async Task<ActionResult<IEnumerable<Station>>> GetStationsByNameAsync(string name)
    {
        List<Station> stations = new List<Station>();
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(
                "SELECT * FROM Stations WHERE NameFIN = @name",
                conn
            );
            cmd.Parameters.AddWithValue("@name", name);
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
        }
        return stations;
    }

    /// <summary>
    /// It returns a list of stations by address, and it's asynchronous
    /// </summary>
    /// <param name="address">The address of the stations to return.</param>

    public static async Task<ActionResult<IEnumerable<Station>>> GetStationsByAddressAsync(
        string address
    )
    {
        List<Station> stations = new List<Station>();
        using var conn = new MySqlConnection(MySQLHelper.connectionString);
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(
                "SELECT * FROM Stations WHERE AddressFIN = @address",
                conn
            );
            cmd.Parameters.AddWithValue("@address", address);
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
        }
        return stations;
    }
}
