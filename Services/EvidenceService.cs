using System.Text.RegularExpressions;
using CrimeManagementSystem.Models;
using CrimeManagementSystem.Repositories;
using CrimeManagementSystem.Services.Interfaces;

namespace CrimeManagementSystem.Services
{
    public class EvidenceService : IEvidenceService
    {
        private readonly IEvidenceRepository _evidenceRepository;
        private readonly IAuditLogRepository _auditLogRepository; 

        public EvidenceService(IEvidenceRepository evidenceRepository, IAuditLogRepository auditLogRepository)
        {
            _evidenceRepository = evidenceRepository;
            _auditLogRepository = auditLogRepository;
        }

        public async Task<Evidence> AddEvidenceAsync(Evidence evidence)
        {
            if (!IsValidMedia(evidence.Content, evidence.Type))
            {
                throw new Exception("Invalid file format.");
            }

            // Ensure Remarks is properly handled
            evidence.Remarks ??= string.Empty; // Default to empty string if null

            return await _evidenceRepository.AddEvidenceAsync(evidence);
        }

        public async Task<Evidence?> GetEvidenceByIdAsync(int id)
        {
            return await _evidenceRepository.GetEvidenceByIdAsync(id);
        }

        public async Task<(Evidence?, long?)> GetImageEvidenceByIdAsync(int id)
        {
            return await _evidenceRepository.GetImageEvidenceByIdAsync(id);
        }

        public async Task<bool> UpdateEvidenceAsync(int id, string content, string? remarks)
        {
            // Ensure we don't overwrite existing remarks with null
            var existingEvidence = await _evidenceRepository.GetEvidenceByIdAsync(id);
            if (existingEvidence == null) return false;

            remarks ??= existingEvidence.Remarks; // Retain old remarks if null

            return await _evidenceRepository.UpdateEvidenceAsync(id, content, remarks);
        }

        public async Task<bool> SoftDeleteEvidenceAsync(int id)
        {
            var deleted = await _evidenceRepository.SoftDeleteEvidenceAsync(id);
            if (deleted)
            {
                var log = new AuditLog
                {
                    Action = "Soft Delete",
                    EntityType = "Evidence",
                    EntityId = id,
                    Details = $"Evidence with ID {id} was soft deleted."
                };
                await _auditLogRepository.LogActionAsync(log);
            }
            return deleted;
        }

        private bool IsValidMedia(string filePath, string mediaType)
        {
            var allowedExtensions = mediaType switch
            {
                "image" => new HashSet<string> { ".jpg", ".png", ".jpeg", ".gif" },
                "video" => new HashSet<string> { ".mp4", ".avi", ".mov" },
                "audio" => new HashSet<string> { ".mp3", ".wav" },
                "text"  => null, // Text files are always valid
                _ => throw new Exception("Invalid media type.")
            };

            if (allowedExtensions == null) return true; 

            var extension = Path.GetExtension(filePath).ToLower();
            return allowedExtensions.Contains(extension);
        }

        public async Task<bool> HardDeleteEvidenceAsync(int id, string confirmation)
        {
            var evidence = await _evidenceRepository.GetEvidenceByIdAsync(id);
            if (evidence == null || evidence.IsDeleted) return false; // Ensure evidence exists and isn't already soft-deleted.

            if (confirmation != $"DELETE {id}")
            {
                throw new Exception($"Confirmation mismatch. To delete, send: DELETE {id}");
            }

            var deleted = await _evidenceRepository.HardDeleteEvidenceAsync(id);
            if (deleted)
            {
                var log = new AuditLog
                {
                    Action = "Hard Delete",
                    EntityType = "Evidence",
                    EntityId = id,
                    Details = $"Evidence with ID {id} was permanently deleted."
                };
                await _auditLogRepository.LogActionAsync(log);
            }
            return deleted;
        }

        public async Task<Dictionary<string, int>> GetTopWordsInTextEvidenceAsync()
        {
        var textEvidences = await _evidenceRepository.GetAllTextEvidenceAsync();
        
        if (!textEvidences.Any()) return new Dictionary<string, int>(); // No text evidence available

        var stopWords = new HashSet<string> { "and", "the", "to", "is", "in", "of", "for", "on", "with", "a", "an", "this", "that", "at", "by", "it", "as", "are", "be" };

        var wordCount = new Dictionary<string, int>();

        foreach (var evidence in textEvidences)
        {
            var words = Regex.Split(evidence.Content.ToLower(), @"\W+")
                            .Where(w => !string.IsNullOrWhiteSpace(w) && !stopWords.Contains(w))
                            .ToList();

            foreach (var word in words)
            {
                if (wordCount.ContainsKey(word))
                    wordCount[word]++;
                else
                    wordCount[word] = 1;
            }
        }

        return wordCount.OrderByDescending(w => w.Value)
                        .Take(10)
                        .ToDictionary(w => w.Key, w => w.Value);
        }

        public async Task<List<string>> ExtractLinksFromCaseAsync(int caseId)
        {
            var caseEvidences = await _evidenceRepository.GetEvidencesByCaseIdAsync(caseId);

            if (!caseEvidences.Any()) return new List<string>(); // No evidence found for the case

            var urlPattern = @"(https?:\/\/[^\s]+)"; // Regex pattern to detect URLs
            var extractedLinks = new HashSet<string>(); // Use HashSet to avoid duplicates

            foreach (var evidence in caseEvidences)
            {
                var matches = Regex.Matches(evidence.Content, urlPattern);
                foreach (Match match in matches)
                {
                    extractedLinks.Add(match.Value);
                }
            }

            return extractedLinks.ToList();
        }

        public async Task<List<AuditLog>> GetEvidenceAuditLogsAsync()
        {
            return await _auditLogRepository.GetEvidenceAuditLogsAsync();
        }
    }
}
