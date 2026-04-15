using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/tourlogs")]
public class TourLogsController : ControllerBase
{
    private readonly ITourLogsService _logsService;

    public TourLogsController(ITourLogsService service)
    {
        _logsService = service;
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

        var createdTourLog = _logsService.CreateTourLog(tourLog);
        return Created($"api/tourlogs/{createdTourLog.Id}", createdTourLog);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, TourLogs tourLog)
    {
        if (id != tourLog.Id)
        {
            return BadRequest();
        }

        _logsService.UpdateTourLog(tourLog);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _logsService.DeleteTourLog(id);
        return Ok();
    }
}