using Microsoft.EntityFrameworkCore;
using System.Text.Json;

public class TourRepository : ITourRepository
{
    private readonly TourPlannerContext _context;

    public TourRepository(TourPlannerContext context)
    {
        _context = context;
    }

    public List<Tour> GetAll()
    {
        return _context.Tours
            .Include(t => t.TourLogs)
            .ToList();
    }

    public Tour Create(Tour tour, int userId)
    {
        tour.UserId = userId;

        _context.Tours.Add(tour);
        _context.SaveChanges();

        return tour;
    }

    public void Update(Tour tour)
    {
        var existingTour = _context.Tours
            .Include(t => t.TourLogs)
            .FirstOrDefault(t => t.Id == tour.Id);

        if (existingTour != null)
        {
            existingTour.Name = tour.Name;
            existingTour.Description = tour.Description;
            existingTour.From = tour.From;
            existingTour.To = tour.To;
            existingTour.TransportType = tour.TransportType;

            existingTour.TourLogs = tour.TourLogs;

            _context.SaveChanges();
        }
    }
    public void Delete(int id)
    {
        var tour = _context.Tours.Find(id);

        if (tour != null)
        {
            _context.Tours.Remove(tour);
            _context.SaveChanges();
        }
    }
    public List<Tour> SearchTour(string searchTerm)
    {
        return _context.Tours
            .Where(t =>
                t.Name.Contains(searchTerm) ||
                t.Description.Contains(searchTerm))
            .ToList();
    }
    public object ExportTourAndLogsJson()
    {
        return _context.Tours
            .Include(t => t.TourLogs)
            .ToList();
    }
    public object ImportTourAndLogsJson(List<Tour> tours, int userId)
    {
        if (tours == null || !tours.Any())
            return "No data";

        foreach (var tour in tours)
        {
            tour.UserId = userId;

            var logs = tour.TourLogs;
            tour.TourLogs = new List<TourLogs>();

            _context.Tours.Add(tour);
            _context.SaveChanges();

            if (logs != null)
            {
                foreach (var log in logs)
                {
                    log.TourId = tour.Id;
                    _context.TourLogs.Add(log);
                }
            }
        }

        _context.SaveChanges();
        return tours;
    }

    public async Task<List<Tour>> GetToursByUserIdAsync(int userId)
    {
        return await _context.Tours.Where(t => t.UserId == userId).ToListAsync();
    }
}