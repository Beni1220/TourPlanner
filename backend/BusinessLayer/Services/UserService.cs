public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> UsernameExists(string Username)
    {
        if (string.IsNullOrWhiteSpace(Username))
            throw new ArgumentException("Username already exists.");
        return await _userRepository.UsernameExists(Username);
    }
    

    public async Task<User> GetUserByIdAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");
        return await _userRepository.GetUserByIdAsync(userId);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<User> AddUserAsync(User user)
    {
        if (await _userRepository.UsernameExists(user.Username))
            throw new InvalidOperationException("Username already exists.");
        ValidateUser(user);
        return await _userRepository.AddUserAsync(user);
    }

    public async Task UpdateUserAsync(User user)
    {
        if (user.Id <= 0)
            throw new ArgumentException("Invalid user ID.");
        ValidateUser(user);
        await _userRepository.UpdateUserAsync(user);
    }

    public async Task DeleteUserAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");
        await _userRepository.DeleteUserAsync(userId);
    }

    public async Task<User> RegisterUserAsync(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username))
            throw new ArgumentException("Username cannot be empty.");
        if (string.IsNullOrWhiteSpace(user.Password))
            throw new ArgumentException("Password cannot be empty.");
        if (await _userRepository.UsernameExists(user.Username))
            throw new InvalidOperationException("Username already exists.");

        var newUser = new User
        {
            Username = user.Username,
            Password = user.Password,
            CreatedAt = DateTime.UtcNow
        };

        ValidateUser(newUser);
        return await _userRepository.AddUserAsync(newUser);
    }



    private void ValidateUser(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));
        if (string.IsNullOrWhiteSpace(user.Username))
            throw new ArgumentException("Username cannot be empty.");
        if (user.Username.Length > 50)
            throw new ArgumentException("Username is too long.");
        if (string.IsNullOrWhiteSpace(user.Password))
            throw new ArgumentException("Password cannot be empty.");
        if (user.Password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters.");
        if (user.CreatedAt == default)
            throw new ArgumentException("CreatedAt must be set.");
    }
}