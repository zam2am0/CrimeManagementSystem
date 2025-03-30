using System;
using CrimeManagementSystem.Models;

namespace CrimeManagementSystem.Dtos;

public class CaseDetailsDto
{
    public string CaseNumber { get; set; }
    public string CaseName { get; set; }
    public string Description { get; set; }
    public string Area { get; set; }
    public string City { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CaseType { get; set; }
    public string Level { get; set; }
    public string AuthorizationLevel { get; set; }
    public List<AssigneeDto> Assignees { get; set; }
    public List<PersonDto> Persons { get; set; }
    public List<CrimeReportDto> ReportedBy { get; set; }
}
