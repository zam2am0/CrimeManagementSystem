using System;

namespace CrimeManagementSystem.Models;

public class AuditLog
{
    public int Id { get; set; }
    public string Action { get; set; } = string.Empty;
    public int EntityId { get; set; } 
    public string EntityType { get; set; } = "Evidence";
    public string Details { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string PerformedBy { get; set; } // Username or User ID
}
