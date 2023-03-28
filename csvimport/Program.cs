using CsvHelper;
using MySqlConnector;
using dotenv.net;
using System.Globalization;
using csvimport.tests;

namespace csvimport;

class Program
{
    static void Main(string[] args)
    {
        /* Loading the .env file into the environment variables. */
        DotEnv.Load();

        /* Creating an instance of the ImportData class. */
        ImportData importData = new ImportData();

        try
        {
            /* Creating a table(s) in the database. */
            bool success = importData.CreateTable();
            if (!success)
            {
                Console.WriteLine("Failed to create a table(s) in the database!");
                return;
            }
            Console.WriteLine("Remember to import data to the Stations table first!");
            Console.Write(
                "Do you want to import data to the Journeys table or Stations table or Both? (journeys/stations/both) "
            );

            /* Asking the user to enter a valid choice. */
            string? choice = Console.ReadLine();
            while (choice != "journeys" && choice != "stations" && choice != "test" && choice != "both")
            {
                Console.Write("Please enter a valid choice: ");
                choice = Console.ReadLine();
            }

            switch (choice.ToLower())
            {
                case "journeys":
                    importData.ImportJourneysDataAsync().Wait();
                    break;
                case "stations":
                    importData.ImportStationsAsync().Wait();
                    break;
                case "both":
                    importData.ImportStationsAsync().Wait();
                    importData.ImportJourneysDataAsync().Wait();
                    break;
                case "test":
                    ImportTest importTest = new ImportTest();
                    importTest.CreateTable();
                    importTest.ImportStationsAsync().Wait();
                    importTest.ImportJourneysDataAsync().Wait();
                    importTest.GetTableRowCount("JourneysTest");
                    importTest.GetTableRowCount("StationsTest");
                    importTest.DeleteTables();
                    break;
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        catch (MySqlException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
