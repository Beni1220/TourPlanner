public interface ITourRepository
{
    List<Tour> GetAll();
    void Add(Tour tour);
}