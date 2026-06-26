public class TourCoordinateService : ITourCoordinateService
{
    private readonly ITourCoordinateRepository _repository;

    public TourCoordinateService(ITourCoordinateRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<TourCoordinate> GetCoordinatesByTourId(int tourId)
    {
        return _repository.GetCoordinatesByTourId(tourId);
    }


    public void CreateTourCoordinates(IEnumerable<TourCoordinate> tourCoordinates)
    {
        _repository.CreateTourCoordinates(tourCoordinates);
    }

    public bool DoesTourCoordinateExist(int id)
    {
        return _repository.DoesTourCoordinateExist(id);
    }

    public void DeleteTourCoordinate(int id)
    {
        _repository.DeleteTourCoordinate(id);
    }

    public void DeleteTourCoordinatesByTourId(int tourId)
    {
        _repository.DeleteTourCoordinatesByTourId(tourId);
    }
}
