using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    private readonly TokenService _tokenService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService service, TokenService tokenService, ILogger<UserController> logger)
    {
        _service = service;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        }

        // Console.WriteLine($"user: {user}");
        // returnt den Token
        var loggedInUserToken = await _service.RegisterUserAsync(user);
        User userFromDb = await _service.GetUserByUsernameAsync(user.Username);

        // jsonString nicht als Location URI verwenden, sondern die URL des neu erstellten Benutzers zurückgeben
        return Created($"api/users/{userFromDb.Id}", new { token = loggedInUserToken });

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        string loggedInUserToken = await _service.LoginUserAsync(user);
        // Console.WriteLine($"user: {user}");
        // Console.WriteLine($"Presentation Layer ------------- Generated token for user {user.Username}: {loggedInUserToken}");
        return Ok(new { token = loggedInUserToken });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _service.GetAllUsersAsync());
    }


    // löschen
    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        }
        

        var createdUser = await _service.AddUserAsync(user);
        return Created($"api/users/{createdUser.Id}", createdUser);
    }

    // Read User methode hinzufügen

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id,[FromBody] User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        await _service.UpdateUserAsync(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteUserAsync(id);
        return Ok();
    }

    [HttpGet("tours")]
    public async Task<IActionResult> GetToursByUserId([FromHeader(Name = "Authorization")] string token)
    {
        var userId = _tokenService.GetUserIdFromToken(token);
        _logger.LogInformation($"Extracted token test------------------------------------: {token}"); // Debugging line to check the extracted userId
        _logger.LogInformation($"Extracted userId from token: {userId}"); // Debugging line to check the extracted userId
        var tours = await _service.GetToursByUserIdAsync(userId);
        return Ok(tours);
    }
}