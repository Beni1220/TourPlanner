public class TourLogsService : ITourLogsService
{
    private readonly ITourLogsRepository _repository;

    public TourLogsService(ITourLogsRepository repository)
    {
        _repository = repository;
    }

    public List<TourLogs> GetTourLogs() => _repository.GetTourLogs();
    public TourLogs CreateTourLog(TourLogs tourLog) => _repository.CreateTourLog(tourLog);

    public void UpdateTourLog(TourLogs tourLog) => _repository.UpdateTourLog(tourLog);

    public void DeleteTourLog(int id) => _repository.DeleteTourLog(id);
}