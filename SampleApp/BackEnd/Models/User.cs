using BackEnd.Models.Dto;

namespace BackEnd.Models;

/// <summary>
/// Represents a user in the system with authentication and profile information
/// </summary>
public class User
{
    /// <summary>
    /// Unique identifier of the user
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Username for authentication and display
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Email address for communications
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hashed password for secure authentication
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// UTC timestamp of when the user was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// UTC timestamp of the last update to the user
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Converts the domain model to a DTO for API responses, excluding sensitive data
    /// </summary>
    public UserDto ToDto()
    {
        return new UserDto
        {
            Id = Id,
            Username = Username,
            Email = Email,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };
    }
}