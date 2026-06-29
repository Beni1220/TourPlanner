using Moq;

public class TourCoordinateServiceTests
{
    private readonly Mock<ITourCoordinateRepository> _repositoryMock;
    private readonly TourCoordinateService _service;

    public TourCoordinateServiceTests()
    {
        _repositoryMock = new Mock<ITourCoordinateRepository>();
        _service = new TourCoordinateService(_repositoryMock.Object);
    }

    private TourCoordinate CreateValidCoordinate() => new TourCoordinate
    {
        Id = 1,
        TourId = 1,
        Latitude = 48.2092,
        Longitude = 16.3728,
        Sequence = 0
    };

    [Fact]
    public void GetCoordinatesByTourId_ValidId_ReturnsCoordinates()
    {
        var coords = new List<TourCoordinate> { CreateValidCoordinate() };
        _repositoryMock.Setup(r => r.GetCoordinatesByTourId(1)).Returns(coords);
        var result = _service.GetCoordinatesByTourId(1);
        Assert.Single(result);
    }

    [Fact]
    public void GetCoordinatesByTourId_InvalidId_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _service.GetCoordinatesByTourId(0));
    }

    [Fact]
    public void CreateTourCoordinates_ValidCoordinates_CallsRepository()
    {
        var coords = new List<TourCoordinate> { CreateValidCoordinate() };
        _service.CreateTourCoordinates(coords);
        _repositoryMock.Verify(r => r.CreateTourCoordinates(coords), Times.Once);
    }

    [Fact]
    public void CreateTourCoordinates_EmptyList_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _service.CreateTourCoordinates(new List<TourCoordinate>()));
    }

    [Fact]
    public void CreateTourCoordinates_Null_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _service.CreateTourCoordinates(null));
    }

    [Fact]
    public void CreateTourCoordinates_InvalidLatitude_ThrowsArgumentException()
    {
        var coord = CreateValidCoordinate();
        coord.Latitude = 91;
        Assert.Throws<ArgumentException>(() => _service.CreateTourCoordinates(new List<TourCoordinate> { coord }));
    }

    [Fact]
    public void CreateTourCoordinates_InvalidLongitude_ThrowsArgumentException()
    {
        var coord = CreateValidCoordinate();
        coord.Longitude = 181;
        Assert.Throws<ArgumentException>(() => _service.CreateTourCoordinates(new List<TourCoordinate> { coord }));
    }

    [Fact]
    public void CreateTourCoordinates_InvalidTourId_ThrowsArgumentException()
    {
        var coord = CreateValidCoordinate();
        coord.TourId = 0;
        Assert.Throws<ArgumentException>(() => _service.CreateTourCoordinates(new List<TourCoordinate> { coord }));
    }

    [Fact]
    public void CreateTourCoordinates_NegativeSequence_ThrowsArgumentException()
    {
        var coord = CreateValidCoordinate();
        coord.Sequence = -1;
        Assert.Throws<ArgumentException>(() => _service.CreateTourCoordinates(new List<TourCoordinate> { coord }));
    }

    [Fact]
    public void DoesTourCoordinateExist_ValidId_ReturnsTrue()
    {
        _repositoryMock.Setup(r => r.DoesTourCoordinateExist(1)).Returns(true);
        var result = _service.DoesTourCoordinateExist(1);
        Assert.True(result);
    }

    [Fact]
    public void DoesTourCoordinateExist_InvalidId_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _service.DoesTourCoordinateExist(0));
    }

    [Fact]
    public void DeleteTourCoordinate_ValidId_CallsRepository()
    {
        _service.DeleteTourCoordinate(1);
        _repositoryMock.Verify(r => r.DeleteTourCoordinate(1), Times.Once);
    }

    [Fact]
    public void DeleteTourCoordinate_InvalidId_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _service.DeleteTourCoordinate(0));
    }

    [Fact]
    public void DeleteTourCoordinatesByTourId_ValidId_CallsRepository()
    {
        _service.DeleteTourCoordinatesByTourId(1);
        _repositoryMock.Verify(r => r.DeleteTourCoordinatesByTourId(1), Times.Once);
    }

    [Fact]
    public void DeleteTourCoordinatesByTourId_InvalidId_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _service.DeleteTourCoordinatesByTourId(0));
    }
}