namespace Backend_BikeApp.Models;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

public class Journey
{
    public int Id { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ReturnTime { get; set; }
    public string? DepartureStationId { get; set; }
    public string? DepartureStationName { get; set; }

    public string? ReturnStationId { get; set; }

    public string? ReturnStationName { get; set; }

    public int CoveredDistance { get; set; }

    public int Duration { get; set; }
}
