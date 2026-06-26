public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLoginAt { get; set; }
    public List<Tour> Tours { get; set; } = new List<Tour>();

}