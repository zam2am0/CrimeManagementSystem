using System;
using CrimeManagementSystem.Dtos;
using CrimeManagementSystem.Models;

namespace CrimeManagementSystem.Services.Interfaces;

public interface ICaseService
{
    Task<List<Case>> GetAllCasesAsync();
    Task<CaseDetailsDto> GetCaseByIdAsync(int caseId);    
    Task<int> CreateCaseAsync(Case newCase);
    Task<Case> UpdateCaseAsync(int caseId, CaseDetailsDto caseDetailsDto);
    Task<CaseDetailResponseDto> GetCaseDetailsAsync(int caseId);
    Task<List<AssigneeDto>> GetCaseAssigneesAsync(int caseId);
    Task<List<EvidenceDto>> GetCaseEvidenceAsync(int caseId);
    Task<List<PersonDto>> GetCaseSuspectsAsync(int caseId);
    Task<List<PersonDto>> GetCaseVictimsAsync(int caseId);
    Task<List<PersonDto>> GetCaseWitnessesAsync(int caseId);
    Task<CaseReportDto> GetFullCaseDetailsAsync(int caseId);
    Task<byte[]> GenerateCaseReportAsync(int caseId);
    Task<string?> GetCaseStatusByReportIdAsync(string reportId);
}
