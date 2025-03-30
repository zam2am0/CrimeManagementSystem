namespace CrimeManagementSystem.Models
{
    public class Evidence
    {
        public int EvidenceId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // "text" or "image"
        public string Content { get; set; } // Text content or image path
        public string? Remarks { get; set; } // Optional remarks
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false; // Soft delete flag
        // Foreign key to Case
        public int CaseId { get; set; }
        public virtual Case Case { get; set; }
    }
}
