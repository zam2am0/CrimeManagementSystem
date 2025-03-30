using System;

namespace CrimeManagementSystem.Dtos;

public class UpdateEvidenceDto
{
    public string Content { get; set; } = string.Empty;
    public string? Remarks { get; set; }
}
