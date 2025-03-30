using System;
using System.Security.Policy;

namespace CrimeManagementSystem.Models;

public class Case
{
    public int CaseId { get; set; }
    public string CaseNumber { get; set; }
    public string CaseName { get; set; }
    public string Description { get; set; }
    public string Area { get; set; }
    public string City { get; set; }
    public string CaseType { get; set; }
    public string AuthorizationLevel { get; set; }
    public string CaseLevel { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }

    public List<Assignee> Assignees { get; set; }
    public List<Person> Persons { get; set; }
    public List<CrimeReport> CrimeReports { get; set; }

    public virtual ICollection<Evidence> Evidences { get; set; }
    
    
}
