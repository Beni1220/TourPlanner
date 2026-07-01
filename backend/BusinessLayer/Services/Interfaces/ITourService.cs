public interface ITourService
{
    List<Tour> GetAllTours();
    Tour CreateTour(Tour tour, int userId);

    void UpdateTour(Tour tour);

    void DeleteTour(int id);
}