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

        public void Create(Tour tour)
        {
            _context.Tours.Add(tour);
            _context.SaveChanges();
        }
    }