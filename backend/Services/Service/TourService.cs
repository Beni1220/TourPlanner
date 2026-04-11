public class TourService : ITourService
{
    private readonly ITourRepository _repository;

    public TourService(ITourRepository repository)
    {
        _repository = repository;
    }

    public List<Tour> GetAllTours() => _repository.GetAll();
    public void CreateTour(Tour tour) => _repository.Create(tour);
}