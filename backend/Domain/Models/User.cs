using System.Text.Json.Serialization;

public class User
{
    public int Id { get; set; }

    [JsonPropertyName("username")]
    public required string Username { get; set; }

    [JsonPropertyName("password")]
    public required string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLoginAt { get; set; }
    public List<Tour> Tours { get; set; } = new List<Tour>();

}