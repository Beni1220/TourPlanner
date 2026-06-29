public class TourCoordinateService : ITourCoordinateService
{
    private readonly ITourCoordinateRepository _repository;

    public TourCoordinateService(ITourCoordinateRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<TourCoordinate> GetCoordinatesByTourId(int tourId)
    {
        if (tourId <= 0)
            throw new ArgumentException("Invalid tour ID.");
        return _repository.GetCoordinatesByTourId(tourId);
    }

    public void CreateTourCoordinates(IEnumerable<TourCoordinate> tourCoordinates)
    {
        if (tourCoordinates == null || !tourCoordinates.Any())
            throw new ArgumentException("Coordinates cannot be empty.");
        foreach (var coord in tourCoordinates)
            ValidateCoordinate(coord);
        _repository.CreateTourCoordinates(tourCoordinates);
    }

    public bool DoesTourCoordinateExist(int tourId)
    {
        if (tourId <= 0)
            throw new ArgumentException("Invalid tour ID.");
        return _repository.DoesTourCoordinateExist(tourId);
    }

    public void DeleteTourCoordinate(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID.");
        _repository.DeleteTourCoordinate(id);
    }

    public void DeleteTourCoordinatesByTourId(int tourId)
    {
        if (tourId <= 0)
            throw new ArgumentException("Invalid tour ID.");
        _repository.DeleteTourCoordinatesByTourId(tourId);
    }

    private void ValidateCoordinate(TourCoordinate coord)
    {
        if (coord == null)
            throw new ArgumentNullException(nameof(coord));
        if (coord.TourId <= 0)
            throw new ArgumentException("TourId must be greater than 0.");
        if (coord.Latitude < -90 || coord.Latitude > 90)
            throw new ArgumentException("Latitude must be between -90 and 90.");
        if (coord.Longitude < -180 || coord.Longitude > 180)
            throw new ArgumentException("Longitude must be between -180 and 180.");
        if (coord.Sequence < 0)
            throw new ArgumentException("Sequence must be non-negative.");
    }
}