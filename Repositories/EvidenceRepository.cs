using CrimeManagementSystem.Data;
using CrimeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CrimeManagementSystem.Repositories
{
    public class EvidenceRepository : IEvidenceRepository
    {
        private readonly DataContext _context;

        public EvidenceRepository(DataContext context)
        {
            _context = context;
        }

        // Create Evidence
        public async Task<Evidence> AddEvidenceAsync(Evidence evidence)
        {
            // Ensure Remarks is properly handled
            evidence.Remarks = string.IsNullOrWhiteSpace(evidence.Remarks) ? null : evidence.Remarks;

            await _context.Evidences.AddAsync(evidence);
            await _context.SaveChangesAsync();
            return evidence;
        }

        // Retrieve Evidence by ID
        public async Task<Evidence?> GetEvidenceByIdAsync(int id)
        {
            return await _context.Evidences.FirstOrDefaultAsync(e => e.EvidenceId == id && !e.IsDeleted);
        }

        // Retrieve Image Evidence (with size)
        public async Task<(Evidence?, long?)> GetImageEvidenceByIdAsync(int id)
        {
            var evidence = await _context.Evidences.FirstOrDefaultAsync(e => e.EvidenceId == id && e.Type == "image" && !e.IsDeleted);
            if (evidence == null) return (null, null);

            var fileInfo = new FileInfo(evidence.Content);
            return (evidence, fileInfo.Length);
        }

        // Update Evidence (only content and remarks)
        public async Task<bool> UpdateEvidenceAsync(int id, string content, string? remarks)
        {
            var evidence = await _context.Evidences.FirstOrDefaultAsync(e => e.EvidenceId == id && !e.IsDeleted);
            if (evidence == null) return false;

            evidence.Content = content;
            evidence.Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks;
            await _context.SaveChangesAsync();
            return true;
        }

        // Soft Delete Evidence (and log action)
        public async Task<bool> SoftDeleteEvidenceAsync(int id)
        {
            var evidence = await _context.Evidences.FirstOrDefaultAsync(e => e.EvidenceId == id && !e.IsDeleted);
            if (evidence == null) return false;

            evidence.IsDeleted = true;

            // Log deletion
            var auditLog = new AuditLog
            {
                Action = "Soft Delete",
                EntityId = evidence.EvidenceId,
                EntityType = "Evidence",
                Timestamp = DateTime.UtcNow
            };

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();
            return true;
        } 
        public async Task<bool> HardDeleteEvidenceAsync(int id)
        {
            var evidence = await _context.Evidences.FirstOrDefaultAsync(e => e.EvidenceId == id);
            if (evidence == null) return false;

            _context.Evidences.Remove(evidence);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Evidence>> GetAllTextEvidenceAsync()
        {
            return await _context.Evidences
                                .Where(e => e.Type == "text" && !e.IsDeleted)
                                .ToListAsync();
        }

        public async Task<List<Evidence>> GetEvidencesByCaseIdAsync(int caseId)
        {
            return await _context.Evidences
                                .Where(e => e.CaseId == caseId && !e.IsDeleted)
                                .ToListAsync();
        }
    }
}
