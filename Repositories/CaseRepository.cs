using CrimeManagementSystem.Data;
using CrimeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;


namespace CrimeManagementSystem.Repositories
{
    public class CaseRepository : ICaseRepository
    {
        private readonly DataContext _context;

        public CaseRepository(DataContext context)
        {
            _context = context;
        }
        
        public async Task<List<Case>> GetAllCasesAsync()
        {
            var cases = await _context.Cases.ToListAsync();

            return cases.Select(caseItem => new Case
            {
                CaseId = caseItem.CaseId,
                CaseName = caseItem.CaseName,
                Description = TruncateDescription(caseItem.Description),
                CreatedAt = caseItem.CreatedAt
            }).ToList();
        }

        private string TruncateDescription(string description)
        {
            if (string.IsNullOrEmpty(description) || description.Length <= 100)
            {
                return description;
            }

            var truncatedDescription = description.Substring(0, 97);
            var lastSpaceIndex = truncatedDescription.LastIndexOf(" ");

            return lastSpaceIndex > 0 ? truncatedDescription.Substring(0, lastSpaceIndex) + "..." : truncatedDescription + "...";
        }


        public async Task<Case> GetByIdAsync(int caseId)
        {
            return await _context.Cases
                .Include(c => c.Assignees)      // Load Assignees
                .Include(c => c.Persons)        // Load Persons
                .Include(c => c.CrimeReports)   // Load CrimeReports
                .FirstOrDefaultAsync(c => c.CaseId == caseId);
        }

        public async Task<IEnumerable<Case>> GetAllAsync(string searchQuery = null)
        {
            var query = _context.Cases.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(c => c.CaseName.Contains(searchQuery) || c.Description.Contains(searchQuery));
            }

            return await query.ToListAsync();
        }

        public async Task<Case> CreateAsync(Case newCase)
        {
            await _context.Cases.AddAsync(newCase);
            await _context.SaveChangesAsync();
            return newCase;
        }

        public async Task<Case> UpdateAsync(Case updatedCase)
        {
            var existingCase = await _context.Cases.FindAsync(updatedCase.CaseId);
            if (existingCase != null)
            {
                existingCase.CaseName = updatedCase.CaseName;
                existingCase.Description = updatedCase.Description;
                existingCase.Area = updatedCase.Area;
                existingCase.City = updatedCase.City;
                existingCase.CaseType = updatedCase.CaseType;
                existingCase.AuthorizationLevel = updatedCase.AuthorizationLevel;
                existingCase.CaseLevel = updatedCase.CaseLevel;
                existingCase.Assignees = updatedCase.Assignees;
                existingCase.Persons = updatedCase.Persons;
                existingCase.CrimeReports = updatedCase.CrimeReports;

                await _context.SaveChangesAsync();
            }
            return existingCase;
        }

        public async Task<int> SubmitCrimeReportAsync(CrimeReport crimeReport)
        {
            await _context.CrimeReports.AddAsync(crimeReport);
            await _context.SaveChangesAsync();
            return crimeReport.ReportId;
        }

        public async Task<Case?> GetCaseByIdAsync(int caseId)
        {
            return await _context.Cases
                .Include(c => c.Persons)  // Includes suspects, victims, and witnesses
                .Include(c => c.Evidences) // Includes evidence details
                .FirstOrDefaultAsync(c => c.CaseId == caseId);
        }

        public async Task<Case?> GetCaseByReportIdAsync(string reportId)
        {
            return await _context.Cases.FirstOrDefaultAsync(c => c.CaseNumber == reportId);
        }        
}
}
