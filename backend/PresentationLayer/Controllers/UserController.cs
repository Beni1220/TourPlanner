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
    public async Task<IActionResult> Update(int id, User user)
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