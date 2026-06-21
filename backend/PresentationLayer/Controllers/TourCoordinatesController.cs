using Microsoft.AspNetCore.Mvc;

namespace TourPlanner.PresentationLayer.Controllers
{
    /*
    [ApiController]
    [Route("api/[controller]")]
    public class TourCoordinatesController : ControllerBase
    {
        private readonly ITourCoordinateService _service;

        public TourCoordinatesController(ITourCoordinateService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var tourCoordinates = _service.GetCoordinatesByTourId(tourId: 1); // Replace with actual tour ID
            return Ok(tourCoordinates);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var tourCoordinate = _service.GetTourCoordinateById(id);
            if (tourCoordinate == null)
            {
                return NotFound();
            }
            return Ok(tourCoordinate);
        }

        [HttpPost]
        public IActionResult Create(TourCoordinate tourCoordinate)
        {
            if (tourCoordinate == null)
            {
                return BadRequest();
            }

            _service.CreateTourCoordinate(tourCoordinate);
            return CreatedAtAction(nameof(GetById), new { id = tourCoordinate.Id }, tourCoordinate);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, TourCoordinate tourCoordinate)
        {
            if (tourCoordinate == null || id != tourCoordinate.Id)
            {
                return BadRequest();
            }

            var existing = _service.GetTourCoordinateById(id);
            if (existing == null)
            {
                return NotFound();
            }

            _service.UpdateTourCoordinate(tourCoordinate);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _service.GetTourCoordinateById(id);
            if (existing == null)
            {
                return NotFound();
            }

            _service.DeleteTourCoordinate(id);
            return NoContent();
        }
    }
    */
}
