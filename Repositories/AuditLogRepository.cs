using CrimeManagementSystem.Data;
using CrimeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CrimeManagementSystem.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly DataContext _context;

        public AuditLogRepository(DataContext context)
        {
            _context = context;
        }

        public async Task LogActionAsync(AuditLog log)
        {
            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AuditLog>> GetEvidenceAuditLogsAsync()
        {
            return await _context.AuditLogs
                                .Where(log => log.EntityType == "Evidence")
                                .OrderByDescending(log => log.Timestamp)
                                .ToListAsync();
        }
    }
}
