public class TourCoordinateRepository : ITourCoordinateRepository
{
    private readonly TourPlannerContext _context;

    public TourCoordinateRepository(TourPlannerContext context)
    {
        _context = context;
    }

    public IEnumerable<TourCoordinate> GetCoordinatesByTourId(int tourId)
    {
        return _context.TourCoordinates
            .Where(tc => tc.TourId == tourId)
            .OrderBy(tc => tc.Sequence)
            .ToList();
    }



    public void CreateTourCoordinates(IEnumerable<TourCoordinate> tourCoordinates)
    {
        _context.TourCoordinates.AddRange(tourCoordinates);
        _context.SaveChanges();
    }


    public void DeleteTourCoordinate(int id)
    {
        var coordinate = _context.TourCoordinates.Find(id);
        if (coordinate != null)
        {
            _context.TourCoordinates.Remove(coordinate);
            _context.SaveChanges();
        }
    }
}
