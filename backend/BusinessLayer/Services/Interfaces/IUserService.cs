public interface IUserService
{
    Task<bool> UsernameExists(string username);
    Task<User> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);

    Task<User> RegisterUserAsync(User user);
}