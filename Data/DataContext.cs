using CrimeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CrimeManagementSystem.Data;

public class DataContext : DbContext  
{
    public DbSet<User> Users { get; set; }
    public DbSet<Person> Persons { get; set; } 
    public DbSet<Case> Cases { get; set; }
    public DbSet<Assignee> Assignees { get; set; }
    public DbSet<CrimeReport> CrimeReports { get; set; } 
    public DbSet<Evidence> Evidences { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }


    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
