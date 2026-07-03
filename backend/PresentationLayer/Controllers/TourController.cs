using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; 
using System.Security.Claims;             
[ApiController]
[Route("api/tours")]
public class TourController : ControllerBase
{
    private readonly ITourService _service;
    private readonly TokenService _tokenService;
    private readonly ILogger<TourController> _logger;

    public TourController(ITourService service, TokenService tokenService, ILogger<TourController> logger)
    {
        _service = service;
        _tokenService = tokenService;
        _logger = logger;
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


    [HttpGet("search")]
    public IActionResult SearchTour([FromQuery] string searchTerm)
    {
        Console.WriteLine($"Searching for tours with term: {searchTerm}"); // Debugging line to check the search term
        var tours = _service.SearchTour(searchTerm);
        return Ok(tours);
    }

    [HttpGet("token")]
    public async Task<IActionResult> GetToursByUserId([FromHeader(Name = "Authorization")] string token)
    {
        var userId = _tokenService.GetUserIdFromToken(token);
        _logger.LogInformation($"Extracted token test------------------------------------: {token}"); // Debugging line to check the extracted userId
        _logger.LogInformation($"Extracted userId from token: {userId}"); // Debugging line to check the extracted userId
        var tours = await _service.GetToursByUserIdAsync(userId);
        return Ok(tours);
    }
}