namespace CrimeManagementSystem.Dtos
{
    public class CaseDetailResponseDto
    {
        public string CaseNumber { get; set; }
        public string CaseName { get; set; }
        public string Description { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CaseType { get; set; }
        public string Level { get; set; }
        public string AuthorizationLevel { get; set; }
        public List<CrimeReportDto> ReportedBy { get; set; }
        public int NumberOfAssignees { get; set; }
        public int NumberOfEvidences { get; set; }
        public int NumberOfSuspects { get; set; }
        public int NumberOfVictims { get; set; }
        public int NumberOfWitnesses { get; set; }
    }
}
