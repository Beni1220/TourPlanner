using Microsoft.EntityFrameworkCore;

public class TourLogsRepository : ITourLogsRepository
{
    private readonly TourPlannerContext _context;

    public TourLogsRepository(TourPlannerContext context)
    {
        _context = context;
    }

    public List<TourLogs> GetTourLogs()
    {
        return _context.TourLogs.ToList();
    }

    public TourLogs CreateTourLog(TourLogs tourLogs)
    {
        tourLogs.Date = tourLogs.Date.ToUniversalTime();
        _context.TourLogs.Add(tourLogs);
        _context.SaveChanges();
        Console.WriteLine($"Tour-log created with ID: {tourLogs.Id}");
        return tourLogs;
    }

    public void UpdateTourLog(TourLogs tourLogs)
    {
        var existingTourLog = _context.TourLogs.Find(tourLogs.Id);
        if (existingTourLog != null)
        {
            existingTourLog.Date = tourLogs.Date.ToUniversalTime();
            existingTourLog.Comment = tourLogs.Comment;
            existingTourLog.Difficulty = tourLogs.Difficulty;
            existingTourLog.TotalDistance = tourLogs.TotalDistance;
            existingTourLog.TotalTime = tourLogs.TotalTime;
            existingTourLog.Rating = tourLogs.Rating;
            existingTourLog.TourId = tourLogs.TourId;
            _context.SaveChanges();
        }
    }

    public void DeleteTourLog(int id)
    {
        var tourLog = _context.TourLogs.Find(id);
        if (tourLog != null)
        {
            _context.TourLogs.Remove(tourLog);
            _context.SaveChanges();
        }
    }

    public async Task<List<TourLogs>> GetTourLogsByUserIdAsync(int userId)
    {
        List<int> tourIds = await _context.Tours
            .Where(t => t.UserId == userId)
            .Select(t => t.Id)
            .ToListAsync();
        return await _context.TourLogs.Where(t => tourIds.Contains(t.TourId)).ToListAsync();
    }

    public string GetTourNameByTourId(int tourId)
    {
        var tour = _context.Tours.Find(tourId);
        return tour != null ? tour.Name : null;
    }


        public List<TourLogs> SearchTourLogs(string searchTerm)
        {
            return _context.TourLogs
                .Where(tl => tl.Comment.Contains(searchTerm))
                .ToList();
        }

}
