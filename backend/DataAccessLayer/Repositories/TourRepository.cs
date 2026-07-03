<<<<<<< Updated upstream
    using Microsoft.EntityFrameworkCore;
    
    public class TourRepository : ITourRepository
    {
        private readonly TourPlannerContext _context;

        public TourRepository(TourPlannerContext context)
=======
        using Microsoft.EntityFrameworkCore;
        using System.Text.Json;
        public class TourRepository : ITourRepository
>>>>>>> Stashed changes
        {
            private readonly TourPlannerContext _context;

            public TourRepository(TourPlannerContext context)
            {
                _context = context;
            }

            public List<Tour> GetAll()
            {
                return _context.Tours.ToList();
            }

            public Tour Create(Tour tour, int userId)
            {
                tour.UserId = userId; // Set the UserId to the provided value
                // tour.TourDistance = 20; // Placeholder value, replace with actual distance calculation
                _context.Tours.Add(tour);
                _context.SaveChanges();
                Console.WriteLine($"Tour created with ID: {tour.Id}");
                return tour;
            }

<<<<<<< Updated upstream
        public async Task<List<Tour>> GetToursByUserIdAsync(int userId)
        {
            return await _context.Tours.Where(t => t.UserId == userId).ToListAsync();
        }

    }
=======
            public void Update(Tour tour)
            {
                var existingTour = _context.Tours.Find(tour.Id);
                if (existingTour != null)
                {
                    existingTour.Name = tour.Name;
                    existingTour.Description = tour.Description;
                    existingTour.From = tour.From;
                    existingTour.To = tour.To;
                    existingTour.TransportType = tour.TransportType;
                    //existingTour.estimatedTime = tour.estimatedTime; wird automatisch von api berechnet?
                    //existingTour.routeInformation = tour.routeInformation; graphical display für die route
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
                Console.WriteLine($"Searching for tours with term: {searchTerm}");
                return _context.Tours
                    .Where(t => t.Name.Contains(searchTerm) || t.Description.Contains(searchTerm))
                    .ToList();
            }

            public object ExportTourAndLogsJson()
            {
                var tourAndLogs = _context.Tours
                    .Include(t => t.TourLogs)
                    .ToList();    

                return tourAndLogs;

            }

            public object ImportTourAndLogsJson(List<Tour> tours, int userId)
            {
                if (tours == null)
                return "No data";

                foreach (var tour in tours)
                {
                    tour.UserId = userId; 
                    _context.Tours.Add(tour);
                    if (tour.TourLogs != null)
                    {
                        foreach (var log in tour.TourLogs)
                        {
                            log.TourId = tour.Id;
                            _context.TourLogs.Add(log);
                        }
                    }
                }

                _context.SaveChanges();
                return tours;
            }
        }
>>>>>>> Stashed changes
