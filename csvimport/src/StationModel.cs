using CsvHelper.Configuration.Attributes;
using CsvHelper.Configuration;

namespace csvimport;

public class StationCSV
{
    public int FID { get; set; }
    public string? ID { get; set; }
    public string? NameFIN { get; set; }
    public string? NameSWE { get; set; }
    public string? NameENG { get; set; }
    public string? AddressFIN { get; set; }
    public string? AddressSWE { get; set; }
    public string? CityFIN { get; set; }
    public string? CitySWE { get; set; }
    public string? Operator { get; set; }
    public int Capacity { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
}

public class StationMap : ClassMap<StationCSV>
{
    public StationMap()
    {
        Map(m => m.FID).Index(0).Name("FID");
        Map(m => m.ID).Index(1).Name("ID");
        Map(m => m.NameFIN).Index(2).Name("Nimi");
        Map(m => m.NameSWE).Index(3).Name("Namn");
        Map(m => m.NameENG).Index(4).Name("Name");
        Map(m => m.AddressFIN).Index(5).Name("Osoite");
        Map(m => m.AddressSWE).Index(6).Name("Adress");
        Map(m => m.CityFIN).Index(7).Name("Kaupunki");
        Map(m => m.CitySWE).Index(8).Name("Stad");
        Map(m => m.Operator).Index(9).Name("Operaattor");
        Map(m => m.Capacity).Index(10).Name("Kapasiteet");
        Map(m => m.Longitude).Index(11).Name("x");
        Map(m => m.Latitude).Index(12).Name("y");
    }
}
