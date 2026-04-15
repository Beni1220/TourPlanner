public interface ITourRepository
{

    // Tour CRUD
    List<Tour> GetAll();
    Tour Create(Tour tour);

    void Update(Tour tour);

    void Delete(int id);


}