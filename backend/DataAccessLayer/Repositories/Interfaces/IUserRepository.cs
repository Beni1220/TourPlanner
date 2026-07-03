public interface IUserRepository
{
    Task<User> GetUserByUsernameAsync(string username);
    Task<bool> UsernameExistsAsync(string username);
    Task<User> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);

    Task<List<Tour>> GetToursByUserIdAsync(int userId);
}