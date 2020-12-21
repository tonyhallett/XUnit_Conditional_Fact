using Moq;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using XUnit_Conditional_Fact;

namespace Test
{
    public class Skip_Tests: Conditional_Fact_Discoverer_Tests_Base
    {
        [Fact]
        public void Should_Create_A_SkippedTestCase_If_Skip_Logic_Returns_A_Skip_Reason()
        {
            var conditionalFactDiscoverer = new ConditionalFactDiscoverer(mockedMessageSink);

            var skipReason = "A skip reason";
            SetUpSkipLogic(conditionalFactDiscoverer, FakeSkipLogic.Type, skipReason);
            var mockTestCaseFactory = new Mock<ITestCaseFactory>();
            var skipTestCase = new FakeTestCase();
            mockTestCaseFactory.Setup(f => f.CreateSkip(skipReason, mockedMessageSink, mockedTestFrameworkDiscoveryOptions, mockedTestMethod)).Returns(skipTestCase);
            conditionalFactDiscoverer.TestCaseFactory = mockTestCaseFactory.Object;

            var testCases = conditionalFactDiscoverer.Discover(mockedTestFrameworkDiscoveryOptions, mockedTestMethod, GetReflectionAttributeInfo_DiscovererType(null, FakeSkipLogic.Type));
            
            Assert.Single(testCases);
            Assert.Same(skipTestCase,testCases.First());
        }   
    }
    
}
