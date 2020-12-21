using Xunit.Abstractions;
using Xunit.Sdk;

namespace XUnit_Conditional_Fact
{
    internal interface IXunitTestCaseDiscovererProvider
    {
        IXunitTestCaseDiscoverer Provide(ConditionalFact conditionalFact,IMessageSink messageSink);
    }
}
