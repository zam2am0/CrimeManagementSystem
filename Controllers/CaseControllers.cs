using CrimeManagementSystem.Dtos;
using CrimeManagementSystem.Models;
using CrimeManagementSystem.Repositories;
using CrimeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CrimeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseController : ControllerBase
    {
        private readonly ICaseService _caseService;
        private readonly ICrimeReportRepository _crimeReportRepository;

        public CaseController(ICaseService caseService, ICrimeReportRepository crimeReportRepository)
        {
            _caseService = caseService;
            _crimeReportRepository = crimeReportRepository;
        }

        // Get All Cases
        
        [HttpGet]
        public async Task<IActionResult> GetAllCasesAsync()
        {
            var cases = await _caseService.GetAllCasesAsync();
            return Ok(cases);
        }

        // Get case by ID
        [HttpGet("{caseId}")]
        public async Task<IActionResult> GetCaseByIdAsync(int caseId)
        {
            var caseDetails = await _caseService.GetCaseByIdAsync(caseId);
            return caseDetails == null ? NotFound(new { message = "Case not found" }) : Ok(caseDetails);
        }

        // Create a new case, linked to the user who created it
        [HttpPost]
        [Authorize(Roles = "Admin,Investigator")] 
        public async Task<IActionResult> CreateCaseAsync([FromBody] CaseDetailsDto caseDetailsDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User is not authenticated" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid case details", details = ModelState });
            }

            var caseId = await _caseService.CreateAsync(caseDetailsDto, userId);

            if (caseId <= 0)
            {
                return BadRequest(new { message = "Failed to create the case" });
            }

            return CreatedAtAction(nameof(GetCaseByIdAsync), new { caseId }, new { caseId });
        }

        // Update an existing case
        [HttpPut("{caseId}")]
        [Authorize(Roles = "Admin,Investigator")] 
        public async Task<IActionResult> UpdateCaseAsync(int caseId, [FromBody] CaseDetailsDto caseDetailsDto)
        {
            var updatedCase = await _caseService.UpdateCaseAsync(caseId, caseDetailsDto);
            return updatedCase == null ? NotFound(new { message = "Case not found" })
                                        : Ok(updatedCase);
        }

        // Submit a crime report and return the report ID
        [HttpPost("{caseId}/crimeReport")]
        public async Task<IActionResult> SubmitCrimeReportAsync(int caseId, [FromBody] CrimeReportDto crimeReportDto)
        {
            var crimeReport = new CrimeReport
            {
                CaseId = caseId, // Link CrimeReport to Case
                Email = crimeReportDto.Email,
                CivilId = crimeReportDto.CivilId,
                Name = crimeReportDto.Name,
                Role = crimeReportDto.Role
            };

            var reportId = await _crimeReportRepository.SubmitCrimeReportAsync(crimeReport);
            return reportId <= 0 ? BadRequest(new { message = "Failed to submit the crime report" }) 
                                : CreatedAtAction(nameof(GetCrimeReportByIdAsync), new { reportId }, new { reportId });
        }
        // Get a crime report by ID
        [HttpGet("crimeReport/{reportId}")]
        public async Task<IActionResult> GetCrimeReportByIdAsync(int reportId)
        {
            var crimeReport = await _crimeReportRepository.GetCrimeReportByIdAsync(reportId);
            
            if (crimeReport == null)
            {
                return NotFound(new { message = "Crime report not found" });
            }
            
            return Ok(crimeReport);
        }

        // GET: api/Case/5
        [HttpGet("CaseDetails/{id}")]
        [Authorize(Roles = "Admin,Investigator")] 
        public async Task<IActionResult> GetCaseDetails(int id)
        {
            var caseDetails = await _caseService.GetCaseDetailsAsync(id);
            if (caseDetails == null)
            {
                return NotFound();
            }

            return Ok(caseDetails);
        }

        // GET: api/Case/5/assignees
        [HttpGet("{id}/assignees")]
        [Authorize(Roles = "Admin,Investigator")] 
        public async Task<IActionResult> GetCaseAssignees(int id)
        {
            var assignees = await _caseService.GetCaseAssigneesAsync(id);
            if (assignees == null || !assignees.Any())
                return NotFound("No assignees found for this case.");

            return Ok(assignees);
        }

        // GET: api/Case/5/evidence
        [HttpGet("{id}/evidence")]
        [Authorize(Roles = "Admin,Investigator")] 
        public async Task<IActionResult> GetCaseEvidence(int id)
        {
            var evidence = await _caseService.GetCaseEvidenceAsync(id);
            if (evidence == null || !evidence.Any())
                return NotFound("No evidence found for this case.");

            return Ok(evidence);
        }

        // GET: api/Case/5/suspects
        [HttpGet("{id}/suspects")]
        [Authorize(Roles = "Admin,Investigator")] 
        public async Task<IActionResult> GetCaseSuspects(int id)
        {
            var suspects = await _caseService.GetCaseSuspectsAsync(id);
            if (suspects == null || !suspects.Any())
                return NotFound("No suspects found for this case.");

            return Ok(suspects);
        }

        // GET: api/Case/5/victims
        [HttpGet("{id}/victims")]
        [Authorize(Roles = "Admin,Investigator")] 
        public async Task<IActionResult> GetCaseVictims(int id)
        {
            var victims = await _caseService.GetCaseVictimsAsync(id);
            if (victims == null || !victims.Any())
                return NotFound("No victims found for this case.");

            return Ok(victims);
        }

        // GET: api/Case/5/witnesses
        [HttpGet("{id}/witnesses")]
        [Authorize(Roles = "Admin,Investigator")] 
        public async Task<IActionResult> GetCaseWitnesses(int id)
        {
            var witnesses = await _caseService.GetCaseWitnessesAsync(id);
            if (witnesses == null || !witnesses.Any())
                return NotFound("No witnesses found for this case.");

            return Ok(witnesses);
        }

        [HttpGet("{caseId}/report")]
        public async Task<IActionResult> GetCaseReport(int caseId)
        {
            var pdfData = await _caseService.GenerateCaseReportAsync(caseId);
            if (pdfData == null)
            {
                return NotFound("Case not found.");
            }

            return File(pdfData, "application/pdf", $"CaseReport_{caseId}.pdf");
        }

        [HttpGet("status/{reportId}")]
        public async Task<IActionResult> GetCaseStatus(string reportId)
        {
            var status = await _caseService.GetCaseStatusByReportIdAsync(reportId);
            if (status == null)
            {
                return NotFound("Case not found.");
            }

            return Ok(new { reportId, status });
        }
    }
}
