public class TourService : ITourService
{
    private readonly ITourRepository _repository;

    public TourService(ITourRepository repository)
    {
        _repository = repository;
    }

    public List<Tour> GetAllTours()
    {
        return _repository.GetAll();
    }

    public Tour CreateTour(Tour tour, int userId)
    {
        ValidateTour(tour);

        return _repository.Create(tour, userId);
    }

    public void UpdateTour(Tour tour)
    {
        if (tour.Id <= 0)
            throw new ArgumentException("Invalid tour id.");

        ValidateTour(tour);

        _repository.Update(tour);
    }

    public void DeleteTour(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid tour id.");

        _repository.Delete(id);
    }

    private void ValidateTour(Tour tour)
    {
        if (tour == null)
            throw new ArgumentNullException(nameof(tour));

        if (string.IsNullOrWhiteSpace(tour.Name))
            throw new ArgumentException("Tour name is required.");

        if (tour.Name.Length > 100)
            throw new ArgumentException("Tour name is too long.");

        if (string.IsNullOrWhiteSpace(tour.From))
            throw new ArgumentException("Start location is required.");

        if (string.IsNullOrWhiteSpace(tour.To))
            throw new ArgumentException("Destination is required.");

        if (tour.From.Trim().Equals(tour.To.Trim(), StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Start and destination cannot be the same.");

        if (string.IsNullOrWhiteSpace(tour.TransportType))
            throw new ArgumentException("Transport type is required.");

        var validTransportTypes = new[]
        {
            "Hike",
            "Bike",
            "Running",
            "Vacation"
        };

        if (!validTransportTypes.Contains(tour.TransportType))
            throw new ArgumentException("Invalid transport type.");

        if (tour.TourDistance <= 0)
            throw new ArgumentException("Distance must be greater than 0.");

        if (tour.EstimatedTime <= 0)
            throw new ArgumentException("Estimated time must be greater than 0.");
    }
}