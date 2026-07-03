using Microsoft.EntityFrameworkCore;
public class UserRepository : IUserRepository
{
    private readonly TourPlannerContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(TourPlannerContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<User> GetUserByUsernameAsync(string Username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == Username);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> AddUserAsync(User user)
    {
        try
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding user");
            throw;
        }
       
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }



    // löschen und in addUserAsync weiter arbeiten
    public async Task<User> RegisterUserAsync(User user)
    {
        var newUser = new User
        {
            Username = user.Username,
            Password = user.Password
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return newUser;
    }

    public async Task<List<Tour>> GetToursByUserIdAsync(int userId)
    {
        return await _context.Tours.Where(t => t.UserId == userId).ToListAsync();
    }

    


}