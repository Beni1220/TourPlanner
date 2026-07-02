public interface IUserService
{
    Task<User> GetUserByUsernameAsync(string username);

    Task<bool> UsernameExists(string username);
    Task<User> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    // löschen
    Task<User> AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);

    Task<string> RegisterUserAsync(User user);
    Task<string> LoginUserAsync(User user);

    Task<List<Tour>> GetToursByUserIdAsync(int userId);
}