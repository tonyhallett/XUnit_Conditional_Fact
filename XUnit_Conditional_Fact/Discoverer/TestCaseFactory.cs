using Xunit.Abstractions;
using Xunit.Sdk;

namespace XUnit_Conditional_Fact
{
    internal class TestCaseFactory : ITestCaseFactory
    {
        public IXunitTestCase CreateError(string errorReason, IMessageSink diagnosticMessageSink, ITestFrameworkDiscoveryOptions testFrameworkDiscoveryOptions, ITestMethod testMethod)
        {
            return new ExecutionErrorTestCase(diagnosticMessageSink, testFrameworkDiscoveryOptions.MethodDisplayOrDefault(), testFrameworkDiscoveryOptions.MethodDisplayOptionsOrDefault(), testMethod, errorReason);
        }

        public IXunitTestCase CreateSkip(string skipReason, IMessageSink diagnosticMessageSink, ITestFrameworkDiscoveryOptions testFrameworkDiscoveryOptions, ITestMethod testMethod)
        {
            return new SkippedTestCase(skipReason, diagnosticMessageSink, testFrameworkDiscoveryOptions.MethodDisplayOrDefault(), testFrameworkDiscoveryOptions.MethodDisplayOptionsOrDefault(), testMethod);
        }
    }
    
}
