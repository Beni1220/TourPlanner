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
                existingTourLog.Date = tourLogs.Date;
                existingTourLog.Comment = tourLogs.Comment;
                existingTourLog.Difficulty = tourLogs.Difficulty;
                existingTourLog.TotalDistance = tourLogs.TotalDistance;
                existingTourLog.TotalTime = tourLogs.TotalTime;
                existingTourLog.Rating = tourLogs.Rating;
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

    }