using MySqlConnector;
namespace csvimport.tests;

public class ImportTest : ImportData
{
    public override string journeyCSVlocation { get; set; } = @"./tests/datasets";
    public override string stationCSVlocation { get; set; } = @"./tests/datasets/stations";
    public override string journeysDBTableName { get; set; } = "JourneysTest";
    public override string stationsDBTableName { get; set; } = "StationsTest";

    public void DeleteTables() {
        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        using MySqlCommand command = new MySqlCommand("DROP TABLE IF EXISTS JourneysTest; DROP TABLE IF EXISTS StationsTest;", connection);
        command.ExecuteNonQuery();
    }

    public void GetTableRowCount(string name) 
    {
        int count = 0;
        int journeyExpectedCount = 17;
        int stationExpectedCount = 23;
        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        using MySqlCommand command = new MySqlCommand($"SELECT COUNT(*) FROM {name}", connection);
        using MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            count = reader.GetInt32(0);
        }

        if (name == "JourneysTest") {
            if (count == journeyExpectedCount) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"JourneysTest table has {count} rows. Expected {journeyExpectedCount} rows.");
                Console.ResetColor();
            } else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"JourneysTest table has {count} rows. Expected {journeyExpectedCount} rows.");
                Console.ResetColor();
            }
        } else if (name == "StationsTest") {
            if (count == stationExpectedCount) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"StationsTest table has {count} rows. Expected {stationExpectedCount} rows.");
                Console.ResetColor();
            } else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"StationsTest table has {count} rows. Expected {stationExpectedCount} rows.");
                Console.ResetColor();
            }
        }
        
    }

    public override bool CreateTable()
    {
        try
        {
            Console.WriteLine("Creating tables...");

            /* Creating a connection to the database. */
            using var conn = new MySqlConnection(connectionString);

            /* Opening a connection to the database. */
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            /* Getting all the files in the sql folder that end with .sql */
            var files = Directory.GetFiles(@".\tests\sql\", "*.sql");

            /* Creating a table in the database. */
            foreach (var file in files)
            {
                using var cmd = conn.CreateCommand();
                /* Reading the contents of the file and assigning it to the CommandText property of the
                SqlCommand object. */
                cmd.CommandText = File.ReadAllText(file);

                cmd.ExecuteNonQuery();

                Console.WriteLine($"Table from {file} created.");
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

}
