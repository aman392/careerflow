using CareerFlow.Api;
using Microsoft.EntityFrameworkCore;
using Npgsql;
var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("CareerFlow");
if (string.IsNullOrWhiteSpace(connection)) throw new InvalidOperationException("Set ConnectionStrings__CareerFlow before starting the API.");
builder.Services.AddDbContext<CareerDb>(options => options.UseNpgsql(connection));
builder.Services.AddCors(options => options.AddPolicy("LocalFrontend", policy => policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod()));
var app = builder.Build();
app.UseCors("LocalFrontend");
// Stage 1 initializes the schema for an empty local database. Add EF migrations before production hosting.
using (var scope=app.Services.CreateScope()) { await scope.ServiceProvider.GetRequiredService<CareerDb>().Database.EnsureCreatedAsync(); }
app.MapGet("/api/health", () => Results.Ok(new { status="ok" }));
app.MapGet("/api/applications", async (CareerDb db) => await db.Applications.AsNoTracking().OrderByDescending(a=>a.Id).ToListAsync());
app.MapGet("/api/applications/{id:int}", async (int id, CareerDb db) => {
 var item=await db.Applications.AsNoTracking().Include(a=>a.History).Include(a=>a.Interviews).FirstOrDefaultAsync(a=>a.Id==id);
 return item is null ? Results.NotFound() : Results.Ok(item);
});
app.MapPost("/api/applications", async (ApplicationInput input, CareerDb db) => {
 var error=Rules.Validate(input); if(error!=null) return Results.BadRequest(new {message=error});
 var duplicate=await db.Applications.AnyAsync(a=>a.Company.ToLower()==input.Company.Trim().ToLower() && a.Position.ToLower()==input.Position.Trim().ToLower() && a.Status!="Rejected" && a.Status!="Withdrawn");
 if(duplicate) return Results.Conflict(new {message="An active application for this company and position already exists."});
 var item=new JobApplication { Company=input.Company.Trim(), Position=input.Position.Trim(), Location=input.Location?.Trim()??"", Status=input.Status??"Saved", AppliedOn=input.AppliedOn??DateOnly.FromDateTime(DateTime.Today),FollowUpOn=input.FollowUpOn,Notes=input.Notes?.Trim()??""};
 item.History.Add(new StatusEvent {FromStatus="",ToStatus=item.Status});
 db.Applications.Add(item);await db.SaveChangesAsync();return Results.Created($"/api/applications/{item.Id}",item);
});
app.MapPut("/api/applications/{id:int}", async (int id, ApplicationInput input, CareerDb db) => {
 var error=Rules.Validate(input);if(error!=null)return Results.BadRequest(new {message=error});
 var item=await db.Applications.FindAsync(id);if(item is null)return Results.NotFound();
 item.Company=input.Company.Trim();item.Position=input.Position.Trim();item.Location=input.Location?.Trim()??"";item.Notes=input.Notes?.Trim()??"";item.FollowUpOn=input.FollowUpOn;item.AppliedOn=input.AppliedOn??item.AppliedOn;
 await db.SaveChangesAsync();return Results.Ok(item);
});
app.MapPost("/api/applications/{id:int}/status", async (int id, StatusInput input, CareerDb db) => {
 var item=await db.Applications.FindAsync(id);if(item is null)return Results.NotFound();
 if(!Rules.CanChange(item.Status,input.Status))return Results.BadRequest(new {message="This status change isn't allowed."});
 db.StatusEvents.Add(new StatusEvent {JobApplicationId=id,FromStatus=item.Status,ToStatus=input.Status});item.Status=input.Status;await db.SaveChangesAsync();return Results.Ok(item);
});
app.MapPost("/api/applications/{id:int}/interviews", async(int id, InterviewInput input, CareerDb db)=>{
 if(!await db.Applications.AnyAsync(a=>a.Id==id))return Results.NotFound();
 if(string.IsNullOrWhiteSpace(input.Round))return Results.BadRequest(new {message="Interview round is required."});
 var row=new Interview{JobApplicationId=id,Round=input.Round.Trim(),Date=input.Date,Mode=input.Mode,Notes=input.Notes??""};db.Interviews.Add(row);await db.SaveChangesAsync();return Results.Created($"/api/applications/{id}",row);
});
app.MapPost("/api/demo/load", async (CareerDb db)=>{
 if(await db.Applications.AnyAsync())return Results.Conflict(new {message="Demo records can only be loaded into an empty database."});
 var today=DateOnly.FromDateTime(DateTime.Today);
 var samples=new (string company,string position,string location,string status,int dayOffset,int? follow)[] {
 ("Infosys","Associate Software Engineer","Bengaluru","Applied",-18,2),
 ("Accenture","Application Development Associate","Hyderabad","Interview",-25,1),
 ("Cognizant","Programmer Analyst Trainee","Chennai","Under Review",-14,4),
 ("Capgemini","Junior Software Engineer","Pune","Rejected",-31,null),
 ("Deloitte","Technology Analyst","Bengaluru","Saved",-2,null),
 ("TCS","Assistant System Engineer","Mumbai","Applied",-9,-1)
 };
 foreach(var s in samples) { var a=new JobApplication{Company=s.company,Position=s.position,Location=s.location,Status=s.status,AppliedOn=today.AddDays(s.dayOffset),FollowUpOn=s.follow is int n?today.AddDays(n):null,Notes="Sample record"};a.History.Add(new StatusEvent{ToStatus=s.status});db.Applications.Add(a); }
 await db.SaveChangesAsync();return Results.Ok(new{added=samples.Length});
});
app.Run();
public partial class Program { }
