namespace Backend_BikeApp.Models;

public class Station
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
