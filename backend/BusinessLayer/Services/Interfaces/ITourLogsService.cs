public interface ITourLogsService
{
    List<TourLogs> GetTourLogs();
    TourLogs CreateTourLog(TourLogs tourLog);

    void UpdateTourLog(TourLogs tourLog);

    void DeleteTourLog(int id);

    List<TourLogs> SearchTourLogs(string searchTerm);
}