namespace CareerFlow.Api;
public static class Rules {
 public static readonly string[] Statuses = ["Saved","Applied","Under Review","Interview","Offer","Rejected","Withdrawn"];
 public static bool IsActive(string status) => status is not ("Rejected" or "Withdrawn" or "Offer");
 public static bool CanChange(string from, string to) => Statuses.Contains(to) && from != to &&
    !(from is "Rejected" or "Withdrawn" or "Offer");
 public static string? Validate(ApplicationInput input) {
  if(string.IsNullOrWhiteSpace(input.Company) || string.IsNullOrWhiteSpace(input.Position)) return "Company and position are required.";
  if(input.Company.Length > 150 || input.Position.Length > 150) return "Company and position must be 150 characters or fewer.";
  if(input.Status is not null && !Statuses.Contains(input.Status)) return "Choose a valid application status.";
  return null;
 }
}
