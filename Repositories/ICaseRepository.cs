using System;
using CrimeManagementSystem.Models;

namespace CrimeManagementSystem.Repositories;

public interface ICaseRepository
{
    Task<List<Case>> GetAllCasesAsync();
    Task<Case> GetByIdAsync(int caseId);
    Task<IEnumerable<Case>> GetAllAsync(string searchQuery = null);
    Task<int> CreateAsync(Case newCase);
    Task<Case> UpdateAsync(Case updatedCase);
    Task<int> SubmitCrimeReportAsync(CrimeReport crimeReport);
    Task<Case?> GetCaseByIdAsync(int caseId);
    Task<Case?> GetCaseByReportIdAsync(string reportId);

}
