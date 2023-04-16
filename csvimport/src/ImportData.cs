using CsvHelper;
using MySqlConnector;
using System.Globalization;
using System.Net;

namespace csvimport;

/// <summary>
/// Imports the data from the csv file into the database.
/// </summary>

public class ImportData
{
    private readonly ILogger<ImportData> _logger;

    public ImportData(ILogger<ImportData> logger)
    {
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        // _logger.LogInformation("Starting import...");
        // /* Creating a table(s) in the database. */
        // bool success = await CreateTable();
        // if (!success)
        // {
        //     _logger.LogError("Failed to create a table(s) in the database!");
        //     return;
        // }

        await ImportStationsAsync();
        await ImportJourneysDataAsync();
    }

    /* Getting the connection string from the environment variable. */
    public string? connectionString =
        "Server=host.docker.internal;Port=3306;User ID=root;Password=Abc123;Database=bikeapp; default command timeout=600;";

    public List<string> journeyCSVs = new List<string>()
    {
        "https://dev.hsl.fi/citybikes/od-trips-2021/2021-05.csv",
        "https://dev.hsl.fi/citybikes/od-trips-2021/2021-06.csv",
        "https://dev.hsl.fi/citybikes/od-trips-2021/2021-07.csv"
    };

    public List<string> stationCSVs = new List<string>()
    {
        "https://opendata.arcgis.com/datasets/726277c507ef4914b0aec3cbcfcbfafc_0.csv"
    };

    public string journeysDBTableName { get; set; } = "Journeys";

    public string stationsDBTableName { get; set; } = "Stations";

    /// <summary>
    /// It imports the data from the csv file into the database.
    /// </summary>
    public async Task ImportJourneysDataAsync()
    {
        var tasks = new List<Task>();

        List<string> ids = ReturnStationIds();
        List<string> finishedQuery = new List<string>();

        _logger.LogInformation("Started reading the url(s)...");

        /* A foreach loop that is iterating through the files in the files array. */
        foreach (var url in journeyCSVs)
        {
            tasks.Add(
                Task.Run(async () =>
                {
                    /* Creating a list of strings. */
                    var query = new List<string>();

                    int count = 0;

                    /* Creating a list of JourneyCSV objects. */
                    var records = new List<JourneyCSV>();

                    var myClient = new HttpClient(
                        new HttpClientHandler() { UseDefaultCredentials = true }
                    );
                    var response = await myClient.GetAsync(url);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError($"Url: {url} not found.");
                        return;
                    }
                    var streamResponse = await response.Content.ReadAsStreamAsync();

                    StreamReader sr = new StreamReader(streamResponse);

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
                        _logger.LogInformation($"Url: {url} read.");
                    }
                    query = JourneysBatchAsync(query, records);
                    finishedQuery.AddRange(query);

                    records.Clear();

                    _logger.LogInformation($"File: {url} imported.");
                    _logger.LogInformation($"Total valid records: {count} in file {url}.");
                })
            );
        }

        try
        {
            await Task.WhenAll(tasks);

            /* Inserting the records into the database. */
            await InsertJourneysAsync(finishedQuery);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        _logger.LogInformation("Importing journeys data finished.");
    }

    /// <summary>
    /// It inserts the journeys into the database.
    /// </summary>
    /// <param name="MySqlConnection">The connection to the database.</param>
    /// <param name="query">A list of strings that contain the queries to be executed.</param>
    async Task InsertJourneysAsync(List<string> query)
    {
        try
        {
            string sql =
                $"INSERT INTO {journeysDBTableName} (DepartureTime, ReturnTime, DepartureStationId, DepartureStationName, ReturnStationId, ReturnStationName, CoveredDistance, Duration) VALUES "
                + string.Join(",", query);

            await File.WriteAllTextAsync(@"./sql/journeysitems.sql", sql);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
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
        List<string> ids = new List<string>();
        using (var sr = new StreamReader(@"./src/stationids.txt"))
        {
            string[] idsArray = sr.ReadToEnd().Split(',');
            foreach (var id in idsArray)
            {
                ids.Add(id);
            }
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
        /* This if for docker container, because you need "." to double in docker and without docker ","
        I dont know why but this makes sure that it doesnt matter which to use. */
        if (
            !double.TryParse(record.Longitude, out double number)
            && !double.TryParse(record.Latitude, out number)
        )
        {
            record.Longitude = record.Longitude?.Replace(".", ",");
            record.Latitude = record.Latitude?.Replace(".", ",");
        }

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
        catch { }

        // Check if longitude and latitude are valid
        if (
            !int.TryParse(record.FID.ToString(), out int number1)
            || !int.TryParse(record.Capacity.ToString(), out number1)
            || !double.TryParse(record.Longitude, out double number2)
            || !double.TryParse(record.Latitude, out number2)
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
        var tasks = new List<Task>();

        List<string> finishedQuery = new List<string>();

        /* Iterating through the files in the files array. */
        foreach (var url in stationCSVs)
        {
            tasks.Add(
                Task.Run(async () =>
                {
                    /* Creating a list of strings. */
                    var query = new List<string>();

                    int count = 0;

                    /* Creating a list of StationCSV objects. */
                    var records = new List<StationCSV>();

                    var myClient = new HttpClient(
                        new HttpClientHandler() { UseDefaultCredentials = true }
                    );
                    var response = await myClient.GetAsync(url);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError($"Url: {url} not found.");
                        return;
                    }
                    var streamResponse = await response.Content.ReadAsStreamAsync();

                    StreamReader sr = new StreamReader(streamResponse);

                    /* Opening a file and reading it. */

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
                        _logger.LogInformation($"File: {url} read.");
                    }

                    query = StationsBatchAsync(query, records);
                    finishedQuery.AddRange(query);

                    records.Clear();

                    _logger.LogInformation($"File: {url} imported.");
                    _logger.LogInformation($"Total valid records: {count} in file {url}.");
                })
            );
        }

        try
        {
            await Task.WhenAll(tasks);

            /* Inserting the records into the database. */
            await InsertStationsAsync(finishedQuery);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        _logger.LogInformation("Stations imported.");
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
    /// <param name="records">The list of records to insert.</param>
    private async Task InsertStationsAsync(List<string> query)
    {
        try
        {

            string sql =
                $"INSERT INTO {stationsDBTableName} (FID, ID, NameFIN, NameSWE, NameENG, AddressFIN, AddressSWE, CityFIN, CitySWE, Operator, Capacity, Longitude, Latitude) VALUES "
                + string.Join(",", query);
            
            await File.WriteAllTextAsync(@"./sql/stationsitems.sql", sql);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}
