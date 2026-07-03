public class TourService : ITourService
{
    private readonly ITourRepository _repository;
    private readonly ILogger<TourService> _logger;

    public TourService(ITourRepository repository, ILogger<TourService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public List<Tour> GetAllTours()
    {
        return _repository.GetAll();
    }

    public Tour CreateTour(Tour tour, int userId)
    {

        ValidateTour(tour);

        // Log the creation of a new tour with user ID and tour name
        _logger.LogInformation("Creating a new tour for user {UserId}: {TourName}", userId, tour.Name);

        return _repository.Create(tour, userId);
    }

    public void UpdateTour(Tour tour)
    {
        if (tour.Id <= 0)
            throw new ArgumentException("Invalid tour id.");

        ValidateTour(tour);

        _logger.LogInformation("Updating tour {TourId}: {TourName}", tour.Id, tour.Name);

        _repository.Update(tour);
    }

    public void DeleteTour(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid tour id.");

        _logger.LogInformation("Deleting tour {TourId}", id);
        _repository.Delete(id);
    }

    public async Task<List<Tour>> GetToursByUserIdAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");
        return await _repository.GetToursByUserIdAsync(userId);
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

    public List<Tour> SearchTour(string searchTerm)
    {
        Console.WriteLine($"--------------Tourservice -------------- Searching for tours with term: {searchTerm}");
        if (string.IsNullOrWhiteSpace(searchTerm))
            return GetAllTours();

        return _repository.SearchTour(searchTerm);
    }

}