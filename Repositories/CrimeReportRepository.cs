using CrimeManagementSystem.Data;
using CrimeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CrimeManagementSystem.Repositories
{
    public class CrimeReportRepository : ICrimeReportRepository
    {
         private readonly DataContext _context;

        public CrimeReportRepository(DataContext context)
        {
            _context = context;
        }

        // Method to submit a new crime report
        public async Task<int> SubmitCrimeReportAsync(CrimeReport crimeReport)
        {
            // Add the crime report to the database
            await _context.CrimeReports.AddAsync(crimeReport);
            // Save the changes asynchronously
            await _context.SaveChangesAsync();
            // Return the ID of the newly created crime report
            return crimeReport.ReportId;
        }

        // Method to get a crime report by its ID
        public async Task<CrimeReport> GetCrimeReportByIdAsync(int reportId)
        {
            return await _context.CrimeReports
                .FirstOrDefaultAsync(report => report.ReportId == reportId);
        }

    }
}
