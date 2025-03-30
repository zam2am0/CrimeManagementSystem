using System;
using CrimeManagementSystem.Models;

namespace CrimeManagementSystem.Repositories;

public interface IAuditLogRepository
{
    Task LogActionAsync(AuditLog log);
    Task<List<AuditLog>> GetEvidenceAuditLogsAsync();

}
