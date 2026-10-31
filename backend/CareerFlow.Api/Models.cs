using Microsoft.EntityFrameworkCore;
namespace CareerFlow.Api;
public class JobApplication {
 public int Id {get;set;}
 public string Company {get;set;} = "";
 public string Position {get;set;} = "";
 public string Location {get;set;} = "";
 public string Status {get;set;} = "Saved";
 public DateOnly AppliedOn {get;set;} = DateOnly.FromDateTime(DateTime.UtcNow);
 public DateOnly? FollowUpOn {get;set;}
 public string Notes {get;set;} = "";
 public List<StatusEvent> History {get;set;} = new();
 public List<Interview> Interviews {get;set;} = new();
}
public class StatusEvent {
 public int Id {get;set;}
 public int JobApplicationId {get;set;}
 public string FromStatus {get;set;} = "";
 public string ToStatus {get;set;} = "";
 public DateTime ChangedAt {get;set;} = DateTime.UtcNow;
}
public class Interview {
 public int Id {get;set;}
 public int JobApplicationId {get;set;}
 public string Round {get;set;} = "";
 public DateOnly Date {get;set;}
 public string Mode {get;set;} = "Online";
 public string Notes {get;set;} = "";
}
public class CareerDb(DbContextOptions<CareerDb> options) : DbContext(options) {
 public DbSet<JobApplication> Applications => Set<JobApplication>();
 public DbSet<StatusEvent> StatusEvents => Set<StatusEvent>();
 public DbSet<Interview> Interviews => Set<Interview>();
 protected override void OnModelCreating(ModelBuilder builder) {
  builder.Entity<JobApplication>().Property(x=>x.Company).HasMaxLength(150);
  builder.Entity<JobApplication>().Property(x=>x.Position).HasMaxLength(150);
  builder.Entity<JobApplication>().HasMany(x=>x.History).WithOne().HasForeignKey(x=>x.JobApplicationId).OnDelete(DeleteBehavior.Cascade);
  builder.Entity<JobApplication>().HasMany(x=>x.Interviews).WithOne().HasForeignKey(x=>x.JobApplicationId).OnDelete(DeleteBehavior.Cascade);
 }
}
public record ApplicationInput(string Company, string Position, string? Location, string? Status, DateOnly? AppliedOn, DateOnly? FollowUpOn, string? Notes);
public record StatusInput(string Status);
public record InterviewInput(string Round, DateOnly Date, string Mode, string? Notes);
