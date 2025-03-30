using CrimeManagementSystem.Dtos;
using CrimeManagementSystem.Models;
using CrimeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrimeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvidenceController : ControllerBase
    {
        private readonly IEvidenceService _evidenceService;

        public EvidenceController(IEvidenceService evidenceService)
        {
            _evidenceService = evidenceService;
        }

        //Add Evidence (Text or Image)
        [HttpPost]
        public async Task<IActionResult> AddEvidence([FromBody] Evidence evidence)
        {
            try
            {
                // Ensure optional Remarks is handled properly
                evidence.Remarks = string.IsNullOrWhiteSpace(evidence.Remarks) ? null : evidence.Remarks;

                var createdEvidence = await _evidenceService.AddEvidenceAsync(evidence);
                return CreatedAtAction(nameof(GetEvidenceById), new { id = createdEvidence.EvidenceId }, createdEvidence);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Get Evidence by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvidenceById(int id)
        {
            var evidence = await _evidenceService.GetEvidenceByIdAsync(id);
            if (evidence == null) return NotFound("Evidence not found.");
            return Ok(evidence);
        }

        // Get Image Evidence with Size
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetImageEvidenceById(int id)
        {
            var (evidence, size) = await _evidenceService.GetImageEvidenceByIdAsync(id);
            if (evidence == null) return NotFound("Evidence not found or not an image.");
            return Ok(new { evidence, size });
        }

        // Update Evidence (Only Content and Remarks)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvidence(int id, [FromBody] UpdateEvidenceDto updateDto)
        {
            var updated = await _evidenceService.UpdateEvidenceAsync(id, updateDto.Content, updateDto.Remarks);
            if (!updated) return NotFound("Evidence not found.");
            return NoContent();
        }

        // Soft Delete Evidence
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteEvidence(int id)
        {
            var deleted = await _evidenceService.SoftDeleteEvidenceAsync(id);
            if (!deleted) return NotFound("Evidence not found.");
            return NoContent();
        }

        [HttpDelete("hard/{id}")]
        public async Task<IActionResult> HardDeleteEvidence(int id, [FromBody] DeleteConfirmationDto confirmationDto)
        {
            if (confirmationDto.Confirmation?.ToLower() != "yes")
            {
                return BadRequest("Deletion canceled. You must confirm with 'yes'.");
            }

            return Ok($"To finalize, send DELETE {id}");
        }

        [HttpDelete("hard/final/{id}")]
        public async Task<IActionResult> FinalizeHardDeleteEvidence(int id, [FromBody] DeleteConfirmationDto finalDto)
        {
            try
            {
                var deleted = await _evidenceService.HardDeleteEvidenceAsync(id, finalDto.Confirmation = string.Empty);
                if (!deleted) return NotFound("Evidence not found or already deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("text-analysis")]
        public async Task<IActionResult> GetTopWordsInTextEvidence()
        {
            var topWords = await _evidenceService.GetTopWordsInTextEvidenceAsync();
            
            if (topWords.Count == 0)
                return NotFound("No text evidence found.");

            return Ok(topWords);
        }

        [HttpGet("links/{caseId}")]
        public async Task<IActionResult> ExtractLinksFromCase(int caseId)
        {
            var links = await _evidenceService.ExtractLinksFromCaseAsync(caseId);

            if (!links.Any())
                return NotFound("No links found in the evidence for this case.");

            return Ok(links);
        }

        [HttpGet("audit-logs")]
        public async Task<IActionResult> GetEvidenceAuditLogs()
        {
            var logs = await _evidenceService.GetEvidenceAuditLogsAsync();
            
            if (!logs.Any())
                return NotFound("No audit logs found for evidence actions.");

            return Ok(logs);
        }
    }
}
