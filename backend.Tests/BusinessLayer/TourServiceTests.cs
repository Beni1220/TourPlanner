using Moq;

public class TourServiceTests
{
    private readonly Mock<ITourRepository> _repositoryMock;
    private readonly TourService _tourService;

    public TourServiceTests()
    {
        _repositoryMock = new Mock<ITourRepository>();
        _tourService = new TourService(_repositoryMock.Object);
    }

    private Tour CreateValidTour()
    {
        return new Tour
        {
            Id = 1,
            Name = "Vienna Tour",
            Description = "Nice Tour",
            From = "Vienna",
            To = "Graz",
            TransportType = "Hike",
            TourDistance = 200,
            EstimatedTime = 2.5
        };
    }

    [Fact]
    public void CreateTour_ValidTour_ReturnsCreatedTour()
    {
        // Arrange
        var tour = CreateValidTour();

        _repositoryMock
            .Setup(r => r.Create(tour))
            .Returns(tour);

        // Act
        var result = _tourService.CreateTour(tour);

        // Assert
        Assert.Equal(tour, result);
        _repositoryMock.Verify(r => r.Create(tour), Times.Once);
    }

    [Fact]
    public void CreateTour_EmptyName_ThrowsArgumentException()
    {
        var tour = CreateValidTour();
        tour.Name = "";

        Assert.Throws<ArgumentException>(() =>
            _tourService.CreateTour(tour));
    }

    [Fact]
    public void CreateTour_EmptyFrom_ThrowsArgumentException()
    {
        var tour = CreateValidTour();
        tour.From = "";

        Assert.Throws<ArgumentException>(() =>
            _tourService.CreateTour(tour));
    }

    [Fact]
    public void CreateTour_EmptyTo_ThrowsArgumentException()
    {
        var tour = CreateValidTour();
        tour.To = "";

        Assert.Throws<ArgumentException>(() =>
            _tourService.CreateTour(tour));
    }

    [Fact]
    public void CreateTour_SameFromAndTo_ThrowsArgumentException()
    {
        var tour = CreateValidTour();
        tour.To = "Vienna";

        Assert.Throws<ArgumentException>(() =>
            _tourService.CreateTour(tour));
    }

    [Fact]
    public void CreateTour_InvalidTransportType_ThrowsArgumentException()
    {
        var tour = CreateValidTour();
        tour.TransportType = "Plane";

        Assert.Throws<ArgumentException>(() =>
            _tourService.CreateTour(tour));
    }

    [Fact]
    public void CreateTour_DistanceZero_ThrowsArgumentException()
    {
        var tour = CreateValidTour();
        tour.TourDistance = 0;

        Assert.Throws<ArgumentException>(() =>
            _tourService.CreateTour(tour));
    }

    [Fact]
    public void CreateTour_EstimatedTimeZero_ThrowsArgumentException()
    {
        var tour = CreateValidTour();
        tour.EstimatedTime = 0;

        Assert.Throws<ArgumentException>(() =>
            _tourService.CreateTour(tour));
    }

    [Fact]
    public void DeleteTour_ValidId_CallsRepository()
    {
        _tourService.DeleteTour(1);

        _repositoryMock.Verify(r => r.Delete(1), Times.Once);
    }

    [Fact]
    public void DeleteTour_InvalidId_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            _tourService.DeleteTour(0));
    }

    [Fact]
    public void GetAllTours_ReturnsList()
    {
        var tours = new List<Tour>
        {
            CreateValidTour()
        };

        _repositoryMock
            .Setup(r => r.GetAll())
            .Returns(tours);

        var result = _tourService.GetAllTours();

        Assert.Single(result);
        Assert.Equal("Vienna Tour", result[0].Name);
    }
}