namespace Backend_BikeApp.Helpers;
using Backend_BikeApp.Services;
using MySqlConnector;

/// <summary>
/// The class contains a function that returns a list of strings representing station IDs.
/// </summary>
public static class StationIds
{
    /// <summary>
    /// The function returns a list of strings representing station IDs.
    /// </summary>
    /// <returns>A list of strings representing station IDs.</returns>
    public static List<string> ReturnStationIds()
    {
        /* The code is creating a new MySqlConnection object using the connection string provided
        by the MySQLHelper class. The "using" statement ensures that the connection is properly
        disposed of after it is used. */
        using var conn = new MySqlConnection(MySQLHelper.connectionString);

        /* The code is attempting to open a connection to a MySQL database using the
        MySqlConnection object in C#. If an exception is thrown during the attempt to open the
        connection, the error message will be printed to the console. */
        try
        {
            conn.Open();
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }

        /* The code is creating a new MySqlCommand object using the MySqlConnection object in C#. The
        MySqlCommand object is used to execute SQL statements against the MySQL database. */
        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT ID FROM Stations;";
        using var reader = cmd.ExecuteReader();

        /* Creating a new List object to store the station IDs. */
        var ids = new List<string>();

        /* The code is reading the data returned by the SQL query and storing the station IDs in the
        List object. */
        while (reader.Read())
        {
            ids.Add(reader.GetString(0));
        }

        /* Returning the List object containing the station IDs. */
        return ids;
    }
}
