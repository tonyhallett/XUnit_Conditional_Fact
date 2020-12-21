using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using XUnit_Conditional_Fact;

namespace Test
{
    public class Exception_Test: Conditional_Fact_Discoverer_Tests_Base
    {
        class DiscoverException : Exception
        {
            public DiscoverException(string exceptionMessage) : base(exceptionMessage) { }
        }

        [Fact]
        public void Should_Create_Error_Test_Case_If_Exception_Is_Thrown_During_Discoverery()
        {
            var exceptionMessage = "Discovery exception";
            var exception = new DiscoverException(exceptionMessage);

            var messageSink = new Mock<IMessageSink>().Object;
            var testFrameworkDiscoveryOptions = new Mock<ITestFrameworkDiscoveryOptions>().Object;
            var testMethod = new Mock<ITestMethod>().Object;

            var discoverer = new ConditionalFactDiscoverer(messageSink);

            var mockedErrorTestCase = new Mock<IXunitTestCase>().Object;
            var mockTestCaseFactory = new Mock<ITestCaseFactory>();
            mockTestCaseFactory.Setup(tcf => tcf.CreateError(exceptionMessage, messageSink, testFrameworkDiscoveryOptions, testMethod)).Returns(mockedErrorTestCase);
            discoverer.TestCaseFactory = mockTestCaseFactory.Object;

            var mockSkipLogicFactory = new Mock<ISkipLogicFactory>();
            var notSkipLogic = new Mock<ISkipLogic>();
            notSkipLogic.Setup(l => l.GetSkipReason(It.IsAny<TestMethod>())).Returns<string>(null);
            mockSkipLogicFactory.Setup(f => f.CreateOrDefault(It.IsAny<Type>())).Returns(notSkipLogic.Object);
            discoverer.SkipLogicFactory = mockSkipLogicFactory.Object;

            var mockDiscovererProvider = new Mock<IXunitTestCaseDiscovererProvider>();
            mockDiscovererProvider.Setup(p => p.Provide(It.IsAny<ConditionalFact>(), It.IsAny<IMessageSink>())).Throws(exception);
            discoverer.DiscovererProvider = mockDiscovererProvider.Object;

            var attributeInfo = GetReflectionAttributeInfo_DiscovererType(FakeDiscoverer.Type);
            var discoveredTestCases = discoverer.Discover(testFrameworkDiscoveryOptions, testMethod, attributeInfo);
            Assert.Single(discoveredTestCases);
            Assert.Same(mockedErrorTestCase, discoveredTestCases.First());

            

        }

    }
}
