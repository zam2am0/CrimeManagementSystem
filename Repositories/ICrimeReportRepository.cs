using System;
using CrimeManagementSystem.Models;

namespace CrimeManagementSystem.Repositories;

public interface ICrimeReportRepository
{
    Task<int> SubmitCrimeReportAsync(CrimeReport crimeReport);
    Task<CrimeReport> GetCrimeReportByIdAsync(int reportId);

}
