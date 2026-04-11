public class TourService : ITourService
{
    private readonly ITourRepository _repository;

    public TourService(ITourRepository repository)
    {
        _repository = repository;
    }

    public List<Tour> GetAllTours() => _repository.GetAll();
    public Tour CreateTour(Tour tour) => _repository.Create(tour);

    public void UpdateTour(Tour tour) => _repository.Update(tour);

    public void DeleteTour(int id) => _repository.Delete(id);
}