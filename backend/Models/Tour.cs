public class Tour
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Category { get; set; }
    public int DistanceKm { get; set; }
    public int DurationMinutes { get; set; }
}