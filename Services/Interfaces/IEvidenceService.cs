using System;
using CrimeManagementSystem.Models;

namespace CrimeManagementSystem.Services.Interfaces;

public interface IEvidenceService
{
    Task<Evidence> AddEvidenceAsync(Evidence evidence);
    Task<Evidence?> GetEvidenceByIdAsync(int id);
    Task<(Evidence?, long?)> GetImageEvidenceByIdAsync(int id);
    Task<bool> UpdateEvidenceAsync(int id, string content, string? remarks);
    Task<bool> SoftDeleteEvidenceAsync(int id);
    Task<bool> HardDeleteEvidenceAsync(int id, string confirmation);
    Task<Dictionary<string, int>> GetTopWordsInTextEvidenceAsync();
    Task<List<string>> ExtractLinksFromCaseAsync(int caseId);
    Task<List<AuditLog>> GetEvidenceAuditLogsAsync();
    
}
