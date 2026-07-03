using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; 
using System.Security.Claims;             
[ApiController]
[Route("api/tours")]
public class TourController : ControllerBase
{
    private readonly ITourService _service;
    private readonly TokenService _tokenService;

    public TourController(ITourService service, TokenService tokenService)
    {
        _service = service;
        _tokenService = tokenService;   
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_service.GetAllTours());
    }

    
    [HttpPost]
    public IActionResult Create(Tour tour, [FromHeader(Name = "Authorization")] string token)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        }

        try
        {
            Console.WriteLine($"Received token: {token}"); //L Debugging line to check the token value

            //var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            int id = _tokenService.GetUserIdFromToken(token); // Validate the token and get the user ID
            //Console.WriteLine($"Extracted user ID from token: {id}"); // Debugging line to check the extracted user ID
            if (id == 0 || string.IsNullOrEmpty(token))
            return Unauthorized(new { message = "Bitte einloggen Sie sich ein."});
            var createdTour = _service.CreateTour(tour, id);
            return Created($"api/tours/{createdTour.Id}", createdTour);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex) // alle anderen Fehler auch abfangen
        {
            return StatusCode(500, new { message = ex.Message });
        }
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