public interface ITourRepository
{
    List<Tour> GetAll();
    void Create(Tour tour);
}