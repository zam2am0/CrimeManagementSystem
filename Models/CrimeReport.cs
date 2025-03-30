using System;
using System.ComponentModel.DataAnnotations;

namespace CrimeManagementSystem.Models;

public class CrimeReport
{
    [Key]
    public int ReportId { get; set; }
    public int CaseId { get; set; } 
    public string Name{ get; set; }
    public string ReportedBy { get; set; }
    public string CivilId { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Description { get; set; }  // A detailed description of the crime
    public DateTime CreatedAt { get; set; }  // The date and time the report was created

    // Navigation property for Case
    public Case Case { get; set; }
    
}
