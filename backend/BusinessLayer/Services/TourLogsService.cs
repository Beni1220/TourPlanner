public class TourLogsService : ITourLogsService
{
    private readonly ITourLogsRepository _repository;

    public TourLogsService(ITourLogsRepository repository)
    {
        _repository = repository;
    }

    public List<TourLogs> GetTourLogs() => _repository.GetTourLogs();

    public TourLogs CreateTourLog(TourLogs tourLog)
    {
        ValidateTourLog(tourLog);
        return _repository.CreateTourLog(tourLog);
    }

    public void UpdateTourLog(TourLogs tourLog)
    {
        ValidateTourLog(tourLog);
        _repository.UpdateTourLog(tourLog);
    }

    public void DeleteTourLog(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID.");
        _repository.DeleteTourLog(id);
    }

    private void ValidateTourLog(TourLogs tourLog)
    {
        if (tourLog == null)
            throw new ArgumentNullException(nameof(tourLog));
        if (tourLog.TourId <= 0)
            throw new ArgumentException("TourId must be greater than 0.");
        if (string.IsNullOrWhiteSpace(tourLog.Comment))
            throw new ArgumentException("Comment cannot be empty.");
        // if (tourLog.Difficulty < 1 || tourLog.Difficulty > 5)
        //     throw new ArgumentException("Difficulty must be between 1 and 5.");
        if (tourLog.Rating < 1 || tourLog.Rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5.");
        if (tourLog.TotalDistance <= 0)
            throw new ArgumentException("TotalDistance must be greater than 0.");
        if (tourLog.TotalTime <= 0)
            throw new ArgumentException("TotalTime must be greater than 0.");
        if (tourLog.Date == default)
            throw new ArgumentException("Date must be set.");
    }
}