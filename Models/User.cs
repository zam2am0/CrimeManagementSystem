using System;

namespace CrimeManagementSystem.Models;

public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; } // Make sure to hash the password
    public int RoleId { get; set; }
    public Role Role { get; set; } // Navigation property to Role
}
