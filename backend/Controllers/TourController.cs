using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/tours")]
public class TourController : ControllerBase
{
    private readonly ITourService _service;

    public TourController(ITourService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_service.GetAllTours());
    }

    [HttpPost]
    public IActionResult Create(Tour tour)
    {
        _service.CreateTour(tour);
        return Ok();
    }
}