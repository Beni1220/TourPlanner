  public interface ITourLogsRepository
{
    // Tour-logs CRUD
    List<TourLogs> GetTourLogs();
    TourLogs CreateTourLog(TourLogs tourLogs);
    void UpdateTourLog(TourLogs tourLogs);
    void DeleteTourLog(int id);
<<<<<<< HEAD
    Task<List<TourLogs>> GetTourLogsByUserIdAsync(int userId);
    string GetTourNameByTourId(int tourId);
=======
    List<TourLogs> SearchTourLogs(string searchTerm);
>>>>>>> 1a1b701c795be69959da1f78ddae8ab81ec0201d
}
    