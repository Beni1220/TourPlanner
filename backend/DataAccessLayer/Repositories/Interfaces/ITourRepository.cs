public interface ITourRepository
{

    // Tour CRUD
    List<Tour> GetAll();
    Tour Create(Tour tour, int userId);

    void Update(Tour tour);

    void Delete(int id);

    List<Tour> SearchTour(string searchTerm);

<<<<<<< Updated upstream
    Task<List<Tour>> GetToursByUserIdAsync(int userId);
=======
    object ExportTourAndLogsJson();

    object ImportTourAndLogsJson(List<Tour> tours, int userId);


>>>>>>> Stashed changes
}