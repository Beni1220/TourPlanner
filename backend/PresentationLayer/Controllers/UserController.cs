using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        }

        Console.WriteLine($"user: {user}");
        var createdUser = await _service.RegisterUserAsync(user);
        return Created($"api/users/{createdUser.Id}", createdUser);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _service.GetAllUsersAsync());
    }

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
}