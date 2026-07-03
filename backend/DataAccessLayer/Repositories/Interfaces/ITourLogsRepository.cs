  public interface ITourLogsRepository
{
    // Tour-logs CRUD
    List<TourLogs> GetTourLogs();
    TourLogs CreateTourLog(TourLogs tourLogs);
    void UpdateTourLog(TourLogs tourLogs);
    void DeleteTourLog(int id);
    List<TourLogs> SearchTourLogs(string searchTerm);
}
    