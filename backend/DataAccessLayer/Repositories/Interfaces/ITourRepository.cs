public interface ITourRepository
{

    // Tour CRUD
    List<Tour> GetAll();
    Tour Create(Tour tour, int userId);

    void Update(Tour tour);

    void Delete(int id);

    List<Tour> SearchTour(string searchTerm);

    Task<List<Tour>> GetToursByUserIdAsync(int userId);
}