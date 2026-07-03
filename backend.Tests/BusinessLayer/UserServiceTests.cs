using Moq;
using Microsoft.Extensions.Logging;
public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repositoryMock;
    private readonly Mock<TokenService> _tokenServiceMock;
    private readonly Mock<ILogger<UserService>> _loggerMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _tokenServiceMock = new Mock<TokenService>();
        _loggerMock = new Mock<ILogger<UserService>>();
        _userService = new UserService(_repositoryMock.Object, _tokenServiceMock.Object, _loggerMock.Object);
    }

    private User CreateValidUser() => new User
    {
        Id = 1,
        Username = "johndoe",
        Password = "secret123",
        CreatedAt = DateTime.UtcNow,
        LastLoginAt = DateTime.UtcNow
    };

    [Fact]
    public async Task AddUser_ValidUser_ReturnsCreatedUser()
    {
        var user = CreateValidUser();
        _repositoryMock.Setup(r => r.AddUserAsync(user)).ReturnsAsync(user);
        var result = await _userService.AddUserAsync(user);
        Assert.Equal(user, result);
        _repositoryMock.Verify(r => r.AddUserAsync(user), Times.Once);
    }

    [Fact]
    public async Task AddUser_EmptyName_ThrowsArgumentException()
    {
        var user = CreateValidUser();
        user.Username = "";
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.AddUserAsync(user));
    }

    [Fact]
    public async Task AddUser_NameTooLong_ThrowsArgumentException()
    {
        var user = CreateValidUser();
        user.Username = new string('a', 51);
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.AddUserAsync(user));
    }

    [Fact]
    public async Task AddUser_EmptyPassword_ThrowsArgumentException()
    {
        var user = CreateValidUser();
        user.Password = "";
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.AddUserAsync(user));
    }

    [Fact]
    public async Task AddUser_PasswordTooShort_ThrowsArgumentException()
    {
        var user = CreateValidUser();
        user.Password = "abc";
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.AddUserAsync(user));
    }

    [Fact]
    public async Task AddUser_DefaultCreatedAt_ThrowsArgumentException()
    {
        var user = CreateValidUser();
        user.CreatedAt = default;
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.AddUserAsync(user));
    }

    [Fact]
    public async Task GetUserById_ValidId_ReturnsUser()
    {
        var user = CreateValidUser();
        _repositoryMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);
        var result = await _userService.GetUserByIdAsync(1);
        Assert.Equal(user, result);
    }

    [Fact]
    public async Task GetUserById_InvalidId_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.GetUserByIdAsync(0));
    }

    [Fact]
    public async Task UpdateUser_ValidUser_CallsRepository()
    {
        var user = CreateValidUser();
        await _userService.UpdateUserAsync(user);
        _repositoryMock.Verify(r => r.UpdateUserAsync(user), Times.Once);
    }

    [Fact]
    public async Task UpdateUser_InvalidId_ThrowsArgumentException()
    {
        var user = CreateValidUser();
        user.Id = 0;
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.UpdateUserAsync(user));
    }

    [Fact]
    public async Task DeleteUser_ValidId_CallsRepository()
    {
        await _userService.DeleteUserAsync(1);
        _repositoryMock.Verify(r => r.DeleteUserAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteUser_InvalidId_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.DeleteUserAsync(0));
    }

    [Fact]
    public async Task UsernameExists_ValidUsername_ReturnsTrue()
    {
        _repositoryMock.Setup(r => r.UsernameExistsAsync("John Doe")).ReturnsAsync(true);
        var result = await _userService.UsernameExists("John Doe");
        Assert.True(result);
    }

    [Fact]
    public async Task UsernameExists_EmptyUsername_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.UsernameExists(""));
    }

    [Fact]
    public async Task UsernameExists_WhitespaceUsername_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.UsernameExists("   "));
    }

    [Fact]
    public async Task UsernameExists_NullUsername_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.UsernameExists(null));
    }
}