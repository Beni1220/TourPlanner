public interface ITourService
{
    List<Tour> GetAllTours();
    Tour CreateTour(Tour tour);

    void UpdateTour(Tour tour);

    void DeleteTour(int id);
}