using Xunit.Abstractions;

namespace XUnit_Conditional_Fact
{
    public interface ISkipLogic
    {
        string GetSkipReason(ITestMethod testMethod);
    }
}
