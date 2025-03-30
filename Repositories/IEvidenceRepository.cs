using System;
using CrimeManagementSystem.Models;
namespace CrimeManagementSystem.Repositories;

public interface IEvidenceRepository
{
    Task<Evidence> AddEvidenceAsync(Evidence evidence);
    Task<Evidence?> GetEvidenceByIdAsync(int id);
    Task<(Evidence?, long?)> GetImageEvidenceByIdAsync(int id);
    Task<bool> UpdateEvidenceAsync(int id, string content, string? remarks);
    Task<bool> SoftDeleteEvidenceAsync(int id);
    Task<bool> HardDeleteEvidenceAsync(int id);
    Task<List<Evidence>> GetAllTextEvidenceAsync();
    Task<List<Evidence>> GetEvidencesByCaseIdAsync(int caseId);
}
