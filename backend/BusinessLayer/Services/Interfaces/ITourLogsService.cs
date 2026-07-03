public interface ITourLogsService
{
    List<TourLogs> GetTourLogs();
    TourLogs CreateTourLog(TourLogs tourLog);

    void UpdateTourLog(TourLogs tourLog);

    void DeleteTourLog(int id);

     Task<List<TourLogs>> GetTourLogsByUserIdAsync(int userId);

    string GetTourNameByTourId(int tourId);
}