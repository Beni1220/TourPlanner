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
        var createdTour = _service.CreateTour(tour);
        return Created($"api/tours/{createdTour.Id}", createdTour);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Tour tour)
    {
        if (id != tour.Id)
        {
            return BadRequest();
        }

        _service.UpdateTour(tour);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _service.DeleteTour(id);
        return Ok();
    }
}