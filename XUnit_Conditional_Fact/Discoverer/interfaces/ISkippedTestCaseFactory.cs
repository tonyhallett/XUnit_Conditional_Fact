using Xunit.Abstractions;
using Xunit.Sdk;

namespace XUnit_Conditional_Fact
{
    internal interface ITestCaseFactory
    {
        IXunitTestCase CreateSkip(string skipReason, IMessageSink diagnosticMessageSink, ITestFrameworkDiscoveryOptions testFrameworkDiscoveryOptions, ITestMethod testMethod);
        IXunitTestCase CreateError(string errorReason, IMessageSink diagnosticMessageSink, ITestFrameworkDiscoveryOptions testFrameworkDiscoveryOptions, ITestMethod testMethod);
    }
    
}
