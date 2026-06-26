public class Tour
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; }
    public required string From { get; set; }
    public required string To { get; set; }
    public required string TransportType { get; set; }
    public required double TourDistance{ get; set; }
    public required double EstimatedTime{ get; set; }
    //public required string routeInformation { get; set; }

    // tourlogs
    public List<TourLogs> TourLogs { get; set; } = new List<TourLogs>();
    // tour coordinates
    public List<TourCoordinate> TourCoordinates { get; set; } = new List<TourCoordinate>();

    public int UserId { get; set; }
}