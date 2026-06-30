public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;

    public UserService(IUserRepository userRepository, TokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<bool> UsernameExists(string Username)
    {
        if (string.IsNullOrWhiteSpace(Username))
            throw new ArgumentException("Username already exists.");
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

    public async Task<User> AddUserAsync(User user)
    {
        if (await _userRepository.UsernameExistsAsync(user.Username))
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

    public async Task<string> RegisterUserAsync(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username))
            throw new ArgumentException("Username cannot be empty.");
        if (string.IsNullOrWhiteSpace(user.Password))
            throw new ArgumentException("Password cannot be empty.");
        if (await _userRepository.UsernameExistsAsync(user.Username))
            throw new InvalidOperationException("Username already exists.");

        var newUser = new User
        {
            Username = user.Username,
            Password = user.Password,
            CreatedAt = DateTime.UtcNow
        };

        ValidateUser(newUser);
        
        await _userRepository.AddUserAsync(newUser);
        return await LoginUserAsync(newUser);
    }

    public async Task<string> LoginUserAsync(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            throw new ArgumentException("Username or Password cannot be empty.");

        User userFromDb = await _userRepository.GetUserByUsernameAsync(user.Username);
        if (userFromDb != null)
        {
            
            if (userFromDb.Password != user.Password)
                throw new UnauthorizedAccessException("Invalid username or password.");
            
            var token = _tokenService.GenerateToken(user.Id, user.Username);

            return token;

        } 
        else
        {
            throw new UnauthorizedAccessException("user existiert nicht.");
        }
      
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty.");
        return await _userRepository.GetUserByUsernameAsync(username);
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