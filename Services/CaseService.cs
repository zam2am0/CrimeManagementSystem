using CrimeManagementSystem.Dtos;
using CrimeManagementSystem.Models;
using CrimeManagementSystem.Repositories;
using CrimeManagementSystem.Services.Interfaces;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;



namespace CrimeManagementSystem.Services
{
    public class CaseService : ICaseService
    {
        private readonly ICaseRepository _caseRepository;
        private readonly ICrimeReportRepository _crimeReportRepository;

        public CaseService(ICaseRepository caseRepository, ICrimeReportRepository crimeReportRepository)
        {
            _caseRepository = caseRepository;
            _crimeReportRepository = crimeReportRepository;
        }

        public async Task<List<Case>> GetAllCasesAsync()
        {
            return await _caseRepository.GetAllCasesAsync();
        }

        public async Task<CaseDetailsDto> GetCaseByIdAsync(int caseId)
        {
            var caseDetails = await _caseRepository.GetByIdAsync(caseId);
            if (caseDetails == null)
                return null;

            return new CaseDetailsDto
            {
                CaseNumber = caseDetails.CaseNumber,
                CaseName = caseDetails.CaseName,
                Description = TruncateDescription(caseDetails.Description),
                Area = caseDetails.Area,
                City = caseDetails.City,
                CaseType = caseDetails.CaseType,
                Level = caseDetails.CaseLevel,
                AuthorizationLevel = caseDetails.AuthorizationLevel,
                CreatedBy = caseDetails.CreatedBy,
                CreatedAt = caseDetails.CreatedAt,
                Assignees = caseDetails.Assignees.Select(a => new AssigneeDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Role = a.Role
                }).ToList(),
                Persons = caseDetails.Persons.Select(p => new PersonDto
                {
                    Type = p.Type,
                    Name = p.Name,
                    Age = p.Age,
                    Gender = p.Gender,
                    Role = p.Role
                }).ToList(),
                ReportedBy = caseDetails.CrimeReports.Select(cr => new CrimeReportDto
                {
                    ReportId = cr.ReportId,
                    Email = cr.Email,
                    CivilId = cr.CivilId,
                    Name = cr.Name,
                    Role = cr.Role
                }).ToList()
            };
        }

        private string TruncateDescription(string description)
        {
            if (description.Length <= 100) return description;

            int lastSpaceIndex = description.LastIndexOf(' ', 100);
            if (lastSpaceIndex == -1) return description.Substring(0, 100) + "...";

            return description.Substring(0, lastSpaceIndex) + "...";
        }

        public async Task<int> SubmitCrimeReportAsync(CrimeReportDto crimeReportDto)
        {
            var crimeReport = new CrimeReport
            {
                Email = crimeReportDto.Email,
                CivilId = crimeReportDto.CivilId,
                Name = crimeReportDto.Name,
                Role = crimeReportDto.Role
            };
            return await _crimeReportRepository.SubmitCrimeReportAsync(crimeReport);
        }

        // Implement CreateCaseAsync
        public async Task<int> CreateAsync(CaseDetailsDto caseDetailsDto, string userId)
        {
            if (caseDetailsDto == null)
            {
                throw new ArgumentNullException(nameof(caseDetailsDto), "Case details cannot be null");
            }

            var newCase = new Case
            {
                CaseName = caseDetailsDto.CaseName,
                CaseNumber = caseDetailsDto.CaseNumber,
                Description = caseDetailsDto.Description,
                Area = caseDetailsDto.Area,
                City = caseDetailsDto.City,
                CaseType = caseDetailsDto.CaseType,
                CaseLevel = caseDetailsDto.Level,
                AuthorizationLevel = caseDetailsDto.AuthorizationLevel,
                CreatedBy = userId, 
                CreatedAt = DateTime.UtcNow,
                Assignees = caseDetailsDto.Assignees.Select(a => new Assignee { /*  Assignee */ }).ToList(),
                Persons = caseDetailsDto.Persons.Select(p => new Person { /*  Person */ }).ToList(),
                CrimeReports = new List<CrimeReport>() //CrimeReports
            };

            var createdCaseId = await _caseRepository.CreateAsync(newCase);
            
            return createdCaseId;
        }


        // Implement UpdateCaseAsync
        public async Task<Case> UpdateCaseAsync(int caseId, CaseDetailsDto caseDetailsDto)
        {
            if (caseDetailsDto == null)
                throw new ArgumentNullException(nameof(caseDetailsDto));

            var existingCase = await _caseRepository.GetByIdAsync(caseId);
            if (existingCase == null)
                throw new InvalidOperationException("Case not found");

            // Update case properties based on the DTO
            existingCase.CaseName = caseDetailsDto.CaseName;
            existingCase.Description = caseDetailsDto.Description;
            existingCase.Area = caseDetailsDto.Area;
            existingCase.City = caseDetailsDto.City;
            existingCase.CaseType = caseDetailsDto.CaseType;
            existingCase.AuthorizationLevel = caseDetailsDto.AuthorizationLevel;
            existingCase.CaseLevel = caseDetailsDto.Level;

            // Assuming there's a method to update the case in the repository
            var updatedCase = await _caseRepository.UpdateAsync(existingCase);
            return updatedCase;
        }

        public async Task<CaseDetailResponseDto> GetCaseDetailsAsync(int caseId)
        {
            var caseDetails = await _caseRepository.GetByIdAsync(caseId);
            if (caseDetails == null)
                return null;

            var caseDetailResponse = new CaseDetailResponseDto
            {
                CaseNumber = caseDetails.CaseNumber,
                CaseName = caseDetails.CaseName,
                Description = TruncateDescription(caseDetails.Description),
                Area = caseDetails.Area,
                City = caseDetails.City,
                CreatedBy = caseDetails.CreatedBy,
                CreatedAt = caseDetails.CreatedAt,
                CaseType = caseDetails.CaseType,
                Level = caseDetails.CaseLevel,
                AuthorizationLevel = caseDetails.AuthorizationLevel,
                ReportedBy = caseDetails.CrimeReports.Select(cr => new CrimeReportDto
                {
                    ReportId = cr.ReportId,
                    Email = cr.Email,
                    CivilId = cr.CivilId,
                    Name = cr.Name,
                    Role = cr.Role
                }).ToList(),
                NumberOfAssignees = caseDetails.Assignees.Count,
                NumberOfEvidences = caseDetails.Evidences.Count, 
                NumberOfSuspects = caseDetails.Persons.Count(p => p.Type == "Suspect"),
                NumberOfVictims = caseDetails.Persons.Count(p => p.Type == "Victim"),
                NumberOfWitnesses = caseDetails.Persons.Count(p => p.Type == "Witness")
            };

            return caseDetailResponse;
        }

        //All assignees of a case given its ID
        public async Task<List<AssigneeDto>> GetCaseAssigneesAsync(int caseId)
        {
            var caseDetails = await _caseRepository.GetByIdAsync(caseId);
            if (caseDetails == null)
                return null;

            return caseDetails.Assignees.Select(a => new AssigneeDto
            {
                Id = a.Id,
                Name = a.Name,
                Role = a.Role
            }).ToList();
        }

        //All Evidence of a case given its ID
        public async Task<List<EvidenceDto>> GetCaseEvidenceAsync(int caseId)
        {
            var caseDetails = await _caseRepository.GetByIdAsync(caseId);
            if (caseDetails == null)
                return null;

            return caseDetails.Evidences.Select(e => new EvidenceDto
            {
                EvidenceId = e.EvidenceId,
                Description = e.Description,
                FilePath = e.Content
            }).ToList();
        }

        //All Suspect of a case given its ID
        public async Task<List<PersonDto>> GetCaseSuspectsAsync(int caseId)
        {
            var caseDetails = await _caseRepository.GetByIdAsync(caseId);
            if (caseDetails == null)
                return null;

            return caseDetails.Persons
                .Where(p => p.Type == "Suspect")
                .Select(p => new PersonDto
                {
                    Type = p.Type,
                    Name = p.Name,
                    Age = p.Age,
                    Gender = p.Gender,
                    Role = p.Role
                })
                .ToList();
        }

        //All Victims of a case given its ID
        public async Task<List<PersonDto>> GetCaseVictimsAsync(int caseId)
        {
            var caseDetails = await _caseRepository.GetByIdAsync(caseId);
            if (caseDetails == null)
                return null;

            return caseDetails.Persons
                .Where(p => p.Type == "Victim")
                .Select(p => new PersonDto
                {
                    Type = p.Type,
                    Name = p.Name,
                    Age = p.Age,
                    Gender = p.Gender,
                    Role = p.Role
                })
                .ToList();
        }
        
        //All Witnesse of a case given its ID
        public async Task<List<PersonDto>> GetCaseWitnessesAsync(int caseId)
        {
            var caseDetails = await _caseRepository.GetByIdAsync(caseId);
            if (caseDetails == null)
                return null;

            return caseDetails.Persons
                .Where(p => p.Type == "Witness")
                .Select(p => new PersonDto
                {
                    Type = p.Type,
                    Name = p.Name,
                    Age = p.Age,
                    Gender = p.Gender,
                    Role = p.Role
                })
                .ToList();
        }

        public Task<CaseReportDto> GetFullCaseDetailsAsync(int caseId)
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> GenerateCaseReportAsync(int caseId)
        {
            var caseDetails = await _caseRepository.GetCaseByIdAsync(caseId);
            if (caseDetails == null) return null;

            using (MemoryStream ms = new MemoryStream())
            {
                // Initialize PdfWriter and PdfDocument
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Add case details
                document.Add(new Paragraph($"Case Report - {caseDetails.CaseName}"));
                document.Add(new Paragraph($"Case Number: {caseDetails.CaseNumber}"));
                document.Add(new Paragraph($"Description: {caseDetails.Description}"));
                document.Add(new Paragraph($"City: {caseDetails.City}"));

                // Add suspects
                document.Add(new Paragraph("\n--- Suspects ---"));
                foreach (var person in caseDetails.Persons.Where(p => p.Type == "Suspect"))
                {
                    document.Add(new Paragraph($"- {person.Name}, Age: {person.Age}, Role: {person.Role}"));
                }

                // Add victims
                document.Add(new Paragraph("\n--- Victims ---"));
                foreach (var person in caseDetails.Persons.Where(p => p.Type == "Victim"))
                {
                    document.Add(new Paragraph($"- {person.Name}, Age: {person.Age}, Role: {person.Role}"));
                }

                // Add witnesses
                document.Add(new Paragraph("\n--- Witnesses ---"));
                foreach (var person in caseDetails.Persons.Where(p => p.Type == "Witness"))
                {
                    document.Add(new Paragraph($"- {person.Name}, Age: {person.Age}, Role: {person.Role}"));
                }

                // Add evidence
                document.Add(new Paragraph("\n--- Evidence ---"));
                foreach (var evidence in caseDetails.Evidences)
                {
                    document.Add(new Paragraph($"- Evidence ID: {evidence.EvidenceId}, Type: {evidence.Type}, Description: {evidence.Description}"));
                }

                // Close the document to save the changes
                document.Close();

                // Return the byte array of the PDF
                return ms.ToArray();
            }
        }

        public async Task<string?> GetCaseStatusByReportIdAsync(string reportId)
        {
            var caseDetails = await _caseRepository.GetCaseByReportIdAsync(reportId);
            return caseDetails?.CaseLevel; // Return case status (e.g., "Open", "Closed")
        }
    }
}
