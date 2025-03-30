using System;

namespace CrimeManagementSystem.Dtos;

public class CrimeReportDto
{
    public int ReportId { get; set; }
    public string Email { get; set; }
    public string CivilId { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
}
