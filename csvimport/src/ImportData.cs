using CsvHelper;
using MySqlConnector;
using System.Globalization;

namespace csvimport;

/// <summary>
/// Imports the data from the csv file into the database.
/// </summary>

public class ImportData
{
    /* Getting the connection string from the environment variable. */
    public string? connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");

    public virtual string journeyCSVlocation { get; set; } = @".\datasets\";

    public virtual string stationCSVlocation { get; set; } = @".\datasets\stations";

    public virtual string journeysDBTableName { get; set; } = "Journeys";

    public virtual string stationsDBTableName { get; set; } = "Stations";

    /// <summary>
    /// It creates a table.
    /// </summary>
    public virtual bool CreateTable()
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
            var files = Directory.GetFiles(@".\sql\", "*.sql");

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

    /// <summary>
    /// It imports the data from the csv file into the database.
    /// </summary>
    public async Task ImportJourneysDataAsync()
    {
        /* Creating a new connection to the database using the connection string. */
        using var conn = new MySqlConnection(connectionString);

        /* Opening a connection to the database. */
        try
        {
            await conn.OpenAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        /* Getting all the files in the datasets folder that end in .csv */
        string[] files = Directory.GetFiles(journeyCSVlocation, "*.csv");

        var tasks = new List<Task>();

        List<string> ids = ReturnStationIds();
        List<string> finishedQuery = new List<string>();

        Console.WriteLine("Started reading the file...\nThis may take a while... :)");

        /* A foreach loop that is iterating through the files in the files array. */
        foreach (var file in files)
        {
            tasks.Add(
                Task.Run(async () =>
                {
                    /* Creating a list of strings. */
                    var query = new List<string>();

                    int count = 0;

                    /* Creating a list of JourneyCSV objects. */
                    var records = new List<JourneyCSV>();

                    /* Opening a file and reading it. */
                    using (var sr = new StreamReader(file))

                    /* Creating a new instance of the CsvReader class. */
                    using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture))
                    {
                        /* Registering the class map with the context. */
                        csv.Context.RegisterClassMap<JourneyMap>();

                        /* Reading the CSV file line by line. */
                        while (await csv.ReadAsync())
                        {
                            /* Creating a new instance of the JourneyCSV class and assigning the values
                            from the CSV file to the properties of the class. */
                            var record = csv.GetRecord<JourneyCSV>();
                            /* Checking if the journey is valid. */
                            if (IsValidJourney(record!, ids))
                            {
                                /* Adding the record to the records list. */
                                records.Add(record!);
                                count++;
                            }
                        }
                        Console.WriteLine($"File: {file} read.");
                    }
                    query = JourneysBatchAsync(query, records);
                    finishedQuery.AddRange(query);

                    records.Clear();

                    Console.WriteLine($"Total valid records: {count} in {file}");
                })
            );
        }

        try
        {
            await Task.WhenAll(tasks);

            /* Inserting the records into the database. */
            await InsertJourneysAsync(conn, finishedQuery);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        Console.WriteLine("Done.");
    }

    /// <summary>
    /// It inserts the journeys into the database.
    /// </summary>
    /// <param name="MySqlConnection">The connection to the database.</param>
    /// <param name="query">A list of strings that contain the queries to be executed.</param>
    async Task InsertJourneysAsync(MySqlConnection conn, List<string> query)
    {
        string sql =
            $"INSERT INTO {journeysDBTableName} (DepartureTime, ReturnTime, DepartureStationId, DepartureStationName, ReturnStationId, ReturnStationName, CoveredDistance, Duration) VALUES "
            + string.Join(",", query);
        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// Makes records in list of JourneyCSV objects into a string and adds it to the query list.
    /// </summary>
    /// <param name="query">List of strings.</param>
    /// <param name="records">List of JourneyCSV objects.</param>
    /// <returns>List of strings.</returns>


    private List<string> JourneysBatchAsync(List<string> query, List<JourneyCSV> records)
    {
        foreach (var record in records)
        {
            // add query to list and make sure that date and time values are in the correct format
            query.Add(
                $"('{record.DepartureTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{record.ReturnTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{record.DepartureStationId}', '{record.DepartureStationName}', '{record.ReturnStationId}', '{record.ReturnStationName}', '{record.CoveredDistance}', '{record.Duration}')"
            );
        }
        return query;
    }

    /// <summary>
    /// Returns a list of station ids.
    /// </summary>
    /// <returns>List of station ids.</returns>
    List<string> ReturnStationIds()
    {
        using var conn = new MySqlConnection(connectionString);
        try
        {
            conn.Open();
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT ID FROM {stationsDBTableName};";
        using var reader = cmd.ExecuteReader();
        var ids = new List<string>();
        while (reader.Read())
        {
            ids.Add(reader.GetString(0));
        }
        return ids;
    }

    /// <summary>
    /// Checks if the record is valid.
    /// </summary>
    /// <param name="JourneyCSV">The type of the record</param>
    /// <param name="ids">List of station ids.</param>
    /// <returns>True if the record is valid, false otherwise.</returns>
    private bool IsValidJourney(JourneyCSV record, List<string> ids)
    {
        if (!ids.Contains(record.DepartureStationId!) && !ids.Contains(record.ReturnStationId!))
        {
            return false;
        }

        record.DepartureStationName = record.DepartureStationName?.Replace("'", "''");
        record.ReturnStationName = record.ReturnStationName?.Replace("'", "''");

        if (
            !int.TryParse(record.CoveredDistance, out int number)
            || !int.TryParse(record.Duration, out number)
            || Convert.ToInt32(record.Duration) < 10
            || Convert.ToInt32(record.CoveredDistance) < 10
            || record.DepartureTime == default
            || record.ReturnTime == default
            || record.ReturnTime < record.DepartureTime
            || Convert.ToInt32(record.DepartureStationId) < 0
            || Convert.ToInt32(record.ReturnStationId) < 0
            || Convert.ToInt32(record.CoveredDistance) < 0
            || Convert.ToInt32(record.Duration) < 0
        )
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if the record is valid.
    /// </summary>
    /// <param name="StationCSV">The type of the record.</param>
    /// <returns>True if the station is valid, false otherwise.</returns>
    private bool IsValidStation(StationCSV record)
    {
        /* The above code is replacing the decimal point with a comma. */
        record.Longitude = record.Longitude?.Replace(".", ",");
        record.Latitude = record.Latitude?.Replace(".", ",");
        record.NameFIN = record.NameFIN?.Replace("'", "''");
        record.NameSWE = record.NameSWE?.Replace("'", "''");
        record.NameENG = record.NameENG?.Replace("'", "''");
        record.CityFIN = record.CityFIN?.Replace("'", "''");
        record.CitySWE = record.CitySWE?.Replace("'", "''");
        record.Operator = record.Operator?.Replace("'", "''");
        try
        {
            if (Char.IsWhiteSpace(Convert.ToChar(record.CityFIN!)))
            {
                record.CityFIN = "Helsinki";
            }
            if (Char.IsWhiteSpace(Convert.ToChar(record.CitySWE!)))
            {
                record.CitySWE = "Helsingfors";
            }
            if (Char.IsWhiteSpace(Convert.ToChar(record.Operator!)))
            {
                record.Operator = "CityBike Finland";
            }
        }
        catch (Exception ex) { }

        // Check if longitude and latitude are valid
        if (
            !int.TryParse(record.FID.ToString(), out int number)
            || !int.TryParse(record.Capacity.ToString(), out number)
            || !double.TryParse(record.Longitude, out double number1)
            || !double.TryParse(record.Latitude, out number1)
            || Convert.ToDouble(record.Longitude) < -180
            || Convert.ToDouble(record.Longitude) > 180
            || Convert.ToDouble(record.Latitude) < -90
            || Convert.ToDouble(record.Latitude) > 90
        )
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Imports stations from CSV files to the database.
    /// </summary>

    public async Task ImportStationsAsync()
    {
        /* Creating a new connection to the database using the connection string. */
        using var conn = new MySqlConnection(connectionString);

        /* Opening a connection to the database. */
        try
        {
            await conn.OpenAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        /* Getting all the files in the stations folder that end in .csv */
        var files = Directory.GetFiles(stationCSVlocation, "*.csv");

        var tasks = new List<Task>();

        List<string> finishedQuery = new List<string>();

        /* Iterating through the files in the files array. */
        foreach (var file in files)
        {
            tasks.Add(
                Task.Run(async () =>
                {
                    var count = 0;
                    /* Creating a list of StationCSV objects. */
                    var records = new List<StationCSV>();

                    /* Creating a new query. */
                    List<string> query = new List<string>();

                    /* Opening a file and reading it. */
                    using (var sr = new StreamReader(file))

                    using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture))
                    {
                        /* Registering the class map with the context. */
                        csv.Context.RegisterClassMap<StationMap>();

                        /* Reading the CSV file line by line. */
                        while (await csv.ReadAsync())
                        {
                            /* Creating a new instance of the StationCSV class and assigning the values
                            from the CSV file to the properties of the class. */
                            var record = csv.GetRecord<StationCSV>();

                            /* Checking if the record is valid. */
                            if (IsValidStation(record!))
                            {
                                records.Add(record!);
                                count++;
                            }
                        }
                        Console.WriteLine($"File: {file} read.");
                    }

                    query = StationsBatchAsync(query, records);
                    finishedQuery.AddRange(query);

                    records.Clear();

                    Console.WriteLine($"File: {file} imported.");
                    Console.WriteLine($"Total valid records: {count} in file {file}.");
                })
            );
        }

        try
        {
            await Task.WhenAll(tasks);

            /* Inserting the records into the database. */
            await InsertStationsAsync(conn, finishedQuery);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("Done.");
    }

    /// <summary>
    /// Makes records in list of StationCSV objects into a query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="records">The list of records to insert.</param>
    /// <returns>The query.</returns>

    private List<string> StationsBatchAsync(List<string> query, List<StationCSV> records)
    {
        foreach (var record in records)
        {
            query.Add(
                $"('{record.FID}', '{record.ID}', '{record.NameFIN}', '{record.NameSWE}', '{record.NameENG}', '{record.AddressFIN}', '{record.AddressSWE}', '{record.CityFIN}', '{record.CitySWE}', '{record.Operator}', '{record.Capacity}', '{record.Longitude}', '{record.Latitude}')"
            );
        }
        return query;
    }

    /// <summary>
    /// Inserts a list of stations to the database.
    /// </summary>
    /// <param name="conn">The database connection.</param>
    /// <param name="records">The list of records to insert.</param>
    private async Task InsertStationsAsync(MySqlConnection conn, List<string> query)
    {
        string sql =
            $"INSERT INTO {stationsDBTableName} (FID, ID, NameFIN, NameSWE, NameENG, AddressFIN, AddressSWE, CityFIN, CitySWE, Operator, Capacity, Longitude, Latitude) VALUES "
            + string.Join(",", query);
        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
