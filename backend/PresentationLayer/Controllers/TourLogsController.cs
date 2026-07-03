using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/tourlogs")]
public class TourLogsController : ControllerBase
{
    private readonly ITourLogsService _logsService;
    private readonly TokenService _tokenService;

    public TourLogsController(ITourLogsService service, TokenService tokenService)
    {
        _logsService = service;
        _tokenService = tokenService;
    }

    [HttpGet]
    public IActionResult GetTourLogs()
    {
        return Ok(_logsService.GetTourLogs());
    }

    [HttpPost]
    public IActionResult CreateTourLog(TourLogs tourLog)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        }
        try{
            var createdTourLog = _logsService.CreateTourLog(tourLog);
            return Created($"api/tourlogs/{createdTourLog.Id}", createdTourLog);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, TourLogs tourLog)
    {
        if (id != tourLog.Id)
        {
            return BadRequest();
        }
        try
        {
            _logsService.UpdateTourLog(tourLog);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _logsService.DeleteTourLog(id);
        return Ok();
    }

    [HttpGet("token")]
    public async Task<IActionResult> GetTourLogsByUserId([FromHeader(Name = "Authorization")] string token)
    {
        var userId = _tokenService.GetUserIdFromToken(token);
        if (userId <= 0)
        {
            return Unauthorized(new { message = "Invalid token or user ID." });
        }

        var tourLogs = await _logsService.GetTourLogsByUserIdAsync(userId);
        return Ok(tourLogs);
    }

    [HttpGet("tourName/{id}")]
    public IActionResult GetTourNameByTourId([FromRoute] int id)
    {
        try
        {
            string tourName = _logsService.GetTourNameByTourId(id);
            if (tourName == null)
            {
                return NotFound(new { message = "Tour not found." });
            }
            return Ok(tourName);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}