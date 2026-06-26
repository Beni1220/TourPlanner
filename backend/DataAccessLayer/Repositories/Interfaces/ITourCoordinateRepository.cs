

public interface ITourCoordinateRepository
{
    IEnumerable<TourCoordinate> GetCoordinatesByTourId(int tourId);

    void CreateTourCoordinates(IEnumerable<TourCoordinate> tourCoordinates);
    bool DoesTourCoordinateExist(int id);

    void DeleteTourCoordinate(int id);

    void DeleteTourCoordinatesByTourId(int tourId);
}

