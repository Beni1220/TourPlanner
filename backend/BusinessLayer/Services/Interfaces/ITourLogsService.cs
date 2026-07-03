public interface ITourLogsService
{
    List<TourLogs> GetTourLogs();
    TourLogs CreateTourLog(TourLogs tourLog);

    void UpdateTourLog(TourLogs tourLog);

    void DeleteTourLog(int id);

<<<<<<< HEAD
     Task<List<TourLogs>> GetTourLogsByUserIdAsync(int userId);

    string GetTourNameByTourId(int tourId);
=======
    List<TourLogs> SearchTourLogs(string searchTerm);
>>>>>>> 1a1b701c795be69959da1f78ddae8ab81ec0201d
}