using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;
using XUnit_Conditional_Fact;

namespace Test
{
    public class DiscovererThatUsesOwnAttributeWithReflection : IXunitTestCaseDiscoverer
    {
        private readonly IMessageSink messageSink;

        public DiscovererThatUsesOwnAttributeWithReflection(IMessageSink messageSink)
        {
            this.messageSink = messageSink;
        }
        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            return new IXunitTestCase[] { new SkippedTestCase(((factAttribute as IReflectionAttributeInfo).Attribute as DiscovererThatUsesOwnAttributeWithReflectionAttribute).SomeMethod(), messageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod) };
        }
    }
}
