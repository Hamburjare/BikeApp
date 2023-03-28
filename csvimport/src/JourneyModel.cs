namespace csvimport;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

public class JourneyCSV
{
    public DateTime DepartureTime { get; set; }
    public DateTime ReturnTime { get; set; }
    public string? DepartureStationId { get; set; }
    public string? DepartureStationName { get; set; }

    public string? ReturnStationId { get; set; }

    public string? ReturnStationName { get; set; }

    public string? CoveredDistance { get; set; }

    public string? Duration { get; set; }
}

public class JourneyMap : ClassMap<JourneyCSV>
{
    public JourneyMap()
    {
        Map(m => m.DepartureTime).Index(0).Name("Departure");
        Map(m => m.ReturnTime).Index(1).Name("Return");
        Map(m => m.DepartureStationId).Index(2).Name("Departure station id");
        Map(m => m.DepartureStationName).Index(3).Name("Departure station name");
        Map(m => m.ReturnStationId).Index(4).Name("Return station id");
        Map(m => m.ReturnStationName).Index(5).Name("Return station name");
        Map(m => m.CoveredDistance).Index(6).Name("Covered distance (m)");
        Map(m => m.Duration).Index(7).Name("Duration (sec.)");
    }
}
