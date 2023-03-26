using Backend_BikeApp.Models;
using MySqlConnector;
using Microsoft.AspNetCore.Mvc;


namespace Backend_BikeApp.Services;

public class JourneyService {
    
    public JourneyService() {
    }

    public static async Task<ActionResult<IEnumerable<Journey>>> GetJourneysAsync(string page) {
        List<Journey> journeys = new List<Journey>();
        using var conn = new MySqlConnection(MySQLHelper.connectionString); {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"SELECT * FROM Journeys LIMIT 10 OFFSET {Convert.ToInt32(page) * 10}", conn);
            using (MySqlDataReader reader = await cmd.ExecuteReaderAsync()) {
                while (await reader.ReadAsync()) {
                    Journey journey = new Journey {
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
        return journeys;
    }

}