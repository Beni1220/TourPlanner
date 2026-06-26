using Microsoft.AspNetCore.Mvc;

namespace TourPlanner.PresentationLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TourCoordinatesController : ControllerBase
    {
        private readonly ITourCoordinateService _service;

        public TourCoordinatesController(ITourCoordinateService service)
        {
            _service = service;
        }

        [HttpGet("{tourId}")]
        public IActionResult GetByTourId(int tourId)
        {
            var tourCoordinates = _service.GetCoordinatesByTourId(tourId);

            if (!tourCoordinates.Any())
            {
                return NotFound();
            }

            return Ok(tourCoordinates);
        }

        [HttpPost]
        public IActionResult Create([FromBody] IEnumerable<TourCoordinate> tourCoordinates)
        {
            if (tourCoordinates == null || !tourCoordinates.Any())
            {
                return BadRequest();
            }

            _service.CreateTourCoordinates(tourCoordinates);

            return CreatedAtAction(
                nameof(GetByTourId),
                new { tourId = tourCoordinates.First().TourId },
                tourCoordinates
            );
        }

        [HttpDelete("{tourId}")]
        public IActionResult Delete(int tourId)
        {
            var coords = _service.GetCoordinatesByTourId(tourId);

            if (!coords.Any())
            {
                return NotFound();
            }

            _service.DeleteTourCoordinatesByTourId(tourId);

            return NoContent();
        }
    }
}