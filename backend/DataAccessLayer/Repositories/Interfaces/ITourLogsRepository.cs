  public interface ITourLogsRepository
{
    // Tour-logs CRUD
    List<TourLogs> GetTourLogs();
    TourLogs CreateTourLog(TourLogs tourLogs);
    void UpdateTourLog(TourLogs tourLogs);
    void DeleteTourLog(int id);

    Task<List<TourLogs>> GetTourLogsByUserIdAsync(int userId);
    string GetTourNameByTourId(int tourId);

    List<TourLogs> SearchTourLogs(string searchTerm);
}
    