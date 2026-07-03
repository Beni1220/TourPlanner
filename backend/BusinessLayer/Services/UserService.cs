public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, TokenService tokenService, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<bool> UsernameExists(string Username)
    {
        if (string.IsNullOrWhiteSpace(Username))
            throw new ArgumentException("Username darf nicht leer sein.");
        return await _userRepository.UsernameExistsAsync(Username);
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

    // löschen
    public async Task<User> AddUserAsync(User user)
    {
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

    public async Task<string> RegisterUserAsync(User user)
    {
        
        var newUser = new User
        {
            Username = user.Username,
            Password = user.Password,
            CreatedAt = DateTime.UtcNow
        };
        
        ValidateUser(newUser);

        _logger.LogDebug($"User details: Username={newUser.Username}, CreatedAt={newUser.CreatedAt}");
        
        
        await _userRepository.AddUserAsync(newUser);
        return await LoginUserAsync(newUser);
    }

    public async Task<string> LoginUserAsync(User user)
    {

        if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            throw new ArgumentException("Username and password cannot be empty.");

        User userFromDb = await _userRepository.GetUserByUsernameAsync(user.Username);
        
        if (userFromDb == null)
            throw new UnauthorizedAccessException("Invalid username or password.");
            
        if (userFromDb.Password != user.Password || userFromDb.Username != user.Username)
        {
            _logger.LogWarning($"Login failed for user {user.Username}. Provided password: {user.Password}, Expected password: {userFromDb.Password}");
            throw new UnauthorizedAccessException("Invalid username or password.");
        }
            
        
        var token = _tokenService.GenerateToken(userFromDb.Id, userFromDb.Username);
        // Console.WriteLine($"Business Layer ------------- Generated token for user {userFromDb.Username}: {token}"); // Debugging line to check the generated token
        _logger.LogInformation($"User {userFromDb.Username} logged in successfully");
        return token;

        
       
      
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty.");
        return await _userRepository.GetUserByUsernameAsync(username);
    }

    

    public async Task<List<Tour>> GetToursByUserIdAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");
        return await _userRepository.GetToursByUserIdAsync(userId);
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
        if (_userRepository.UsernameExistsAsync(user.Username).Result)
            throw new InvalidOperationException("Username already exists.");
    }
}