using Moq;

public class TourLogsServiceTests
{
    private readonly Mock<ITourLogsRepository> _repositoryMock;
    private readonly TourLogsService _tourLogsService;

    public TourLogsServiceTests()
    {
        _repositoryMock = new Mock<ITourLogsRepository>();
        _tourLogsService = new TourLogsService(_repositoryMock.Object);
    }

    private TourLogs CreateValidTourLog() => new TourLogs
    {
        Id = 1,
        TourId = 1,
        Date = DateTime.UtcNow,
        Comment = "Great tour!",
        Difficulty = 3,
        TotalDistance = 50.0,
        TotalTime = 2.5,
        Rating = 4.5
    };

    [Fact]
    public void CreateTourLog_ValidLog_ReturnsCreatedLog()
    {
        var log = CreateValidTourLog();
        _repositoryMock.Setup(r => r.CreateTourLog(log)).Returns(log);

        var result = _tourLogsService.CreateTourLog(log);

        Assert.Equal(log, result);
        _repositoryMock.Verify(r => r.CreateTourLog(log), Times.Once);
    }

    [Fact]
    public void CreateTourLog_EmptyComment_ThrowsArgumentException()
    {
        var log = CreateValidTourLog();
        log.Comment = "";

        Assert.Throws<ArgumentException>(() => _tourLogsService.CreateTourLog(log));
    }

    [Fact]
    public void CreateTourLog_DifficultyTooLow_ThrowsArgumentException()
    {
        var log = CreateValidTourLog();
        log.Difficulty = 0;

        Assert.Throws<ArgumentException>(() => _tourLogsService.CreateTourLog(log));
    }

    [Fact]
    public void CreateTourLog_DifficultyTooHigh_ThrowsArgumentException()
    {
        var log = CreateValidTourLog();
        log.Difficulty = 6;

        Assert.Throws<ArgumentException>(() => _tourLogsService.CreateTourLog(log));
    }

    [Fact]
    public void CreateTourLog_RatingTooLow_ThrowsArgumentException()
    {
        var log = CreateValidTourLog();
        log.Rating = 0;

        Assert.Throws<ArgumentException>(() => _tourLogsService.CreateTourLog(log));
    }

    [Fact]
    public void CreateTourLog_RatingTooHigh_ThrowsArgumentException()
    {
        var log = CreateValidTourLog();
        log.Rating = 6;

        Assert.Throws<ArgumentException>(() => _tourLogsService.CreateTourLog(log));
    }

    [Fact]
    public void CreateTourLog_DistanceZero_ThrowsArgumentException()
    {
        var log = CreateValidTourLog();
        log.TotalDistance = 0;

        Assert.Throws<ArgumentException>(() => _tourLogsService.CreateTourLog(log));
    }

    [Fact]
    public void CreateTourLog_TotalTimeZero_ThrowsArgumentException()
    {
        var log = CreateValidTourLog();
        log.TotalTime = 0;

        Assert.Throws<ArgumentException>(() => _tourLogsService.CreateTourLog(log));
    }

    [Fact]
    public void CreateTourLog_InvalidTourId_ThrowsArgumentException()
    {
        var log = CreateValidTourLog();
        log.TourId = 0;

        Assert.Throws<ArgumentException>(() => _tourLogsService.CreateTourLog(log));
    }

    [Fact]
    public void CreateTourLog_DefaultDate_ThrowsArgumentException()
    {
        var log = CreateValidTourLog();
        log.Date = default;

        Assert.Throws<ArgumentException>(() => _tourLogsService.CreateTourLog(log));
    }

    [Fact]
    public void DeleteTourLog_ValidId_CallsRepository()
    {
        _tourLogsService.DeleteTourLog(1);

        _repositoryMock.Verify(r => r.DeleteTourLog(1), Times.Once);
    }

    [Fact]
    public void DeleteTourLog_InvalidId_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _tourLogsService.DeleteTourLog(0));
    }

    [Fact]
    public void GetTourLogs_ReturnsList()
    {
        var logs = new List<TourLogs> { CreateValidTourLog() };
        _repositoryMock.Setup(r => r.GetTourLogs()).Returns(logs);

        var result = _tourLogsService.GetTourLogs();

        Assert.Single(result);
        Assert.Equal("Great tour!", result[0].Comment);
    }
}