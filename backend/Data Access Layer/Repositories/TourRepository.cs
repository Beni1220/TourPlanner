    public class TourRepository : ITourRepository
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

        public Tour Create(Tour tour)
        {
            _context.Tours.Add(tour);
            _context.SaveChanges();
            return tour;
        }

        public void Update(Tour tour)
        {
            var existingTour = _context.Tours.Find(tour.Id);
            if (existingTour != null)
            {
                existingTour.Name = tour.Name;
                existingTour.Category = tour.Category;
                existingTour.DistanceKm = tour.DistanceKm;
                existingTour.DurationMinutes = tour.DurationMinutes;
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

    }