public interface ITourService
{
    List<Tour> GetAllTours();
    Tour CreateTour(Tour tour, int userId);

    void UpdateTour(Tour tour);

    void DeleteTour(int id);

    List<Tour> SearchTour(string searchTerm);

    Task<List<Tour>> GetToursByUserIdAsync(int userId);
    object ExportTourAndLogsJson();

    object ImportTourAndLogsJson(List<Tour> tours, int userId);
}