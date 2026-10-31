using CareerFlow.Api;
using Xunit;
public class RulesTests {
 [Fact] public void MissingCompanyIsRejected() => Assert.NotNull(Rules.Validate(new ApplicationInput("","Developer",null,null,null,null,null)));
 [Fact] public void ARejectedApplicationCannotReopen() => Assert.False(Rules.CanChange("Rejected","Applied"));
 [Fact] public void AppliedToInterviewIsAllowed() => Assert.True(Rules.CanChange("Applied","Interview"));
 [Fact] public void UnknownStatusIsRejected() => Assert.False(Rules.CanChange("Applied","Unknown"));
}
