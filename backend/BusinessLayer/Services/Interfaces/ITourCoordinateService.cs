public interface ITourCoordinateService
{
    IEnumerable<TourCoordinate> GetCoordinatesByTourId(int tourId);
    void CreateTourCoordinates(IEnumerable<TourCoordinate> tourCoordinates);
    void DeleteTourCoordinate(int id);
}
