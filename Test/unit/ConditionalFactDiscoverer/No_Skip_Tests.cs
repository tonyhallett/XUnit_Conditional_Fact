using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using XUnit_Conditional_Fact;

namespace Test
{
    public class No_Skip_Tests: Conditional_Fact_Discoverer_Tests_Base
    {
       
        private IEnumerable<IXunitTestCase> testCasesFromWrappedDiscoverer = new List<IXunitTestCase> { new FakeTestCase() };
        
        [Fact]
        public void Should_Use_The_Wrapped_Discoverer_Based_Upon_The_ConditionalFactAttribute()
        {
            var reflectionAttributeInfo = GetReflectionAttributeInfo_DiscovererType(FakeDiscoverer.Type);

            Discover_No_Skip(reflectionAttributeInfo, null);
        }
        private void Discover_No_Skip(IReflectionAttributeInfo reflectionAttributeInfo, IAttributeInfo expectedAttributeInfo,Action<ConditionalFactDiscoverer> furtherSetup = null)
        {
            var conditionalFact = reflectionAttributeInfo.Attribute as ConditionalFact;

            var conditionalFactDiscoverer = new ConditionalFactDiscoverer(mockedMessageSink);

            SetUpSkipLogic(conditionalFactDiscoverer, It.IsAny<Type>(), null);

            var mockWrappedDiscoverer = new Mock<IXunitTestCaseDiscoverer>();
            if (expectedAttributeInfo == null)
            {
                //this is a gotcha - cannot pass in It.IsAny()!
                mockWrappedDiscoverer.Setup(d => d.Discover(mockedTestFrameworkDiscoveryOptions, mockedTestMethod, It.IsAny<IAttributeInfo>())).Returns(testCasesFromWrappedDiscoverer);
            }
            else
            {
                mockWrappedDiscoverer.Setup(d => d.Discover(mockedTestFrameworkDiscoveryOptions, mockedTestMethod, expectedAttributeInfo)).Returns(testCasesFromWrappedDiscoverer);
            }
            
            var mockDiscovererProvider = new Mock<IXunitTestCaseDiscovererProvider>();
            mockDiscovererProvider.Setup(p => p.Provide(conditionalFact, mockedMessageSink)).Returns(mockWrappedDiscoverer.Object);
            conditionalFactDiscoverer.DiscovererProvider = mockDiscovererProvider.Object;

            if (furtherSetup != null)
            {
                furtherSetup(conditionalFactDiscoverer);
            }
            var testCases = conditionalFactDiscoverer.Discover(mockedTestFrameworkDiscoveryOptions, mockedTestMethod, reflectionAttributeInfo);
            Assert.Equal(testCasesFromWrappedDiscoverer, testCases);
        }

        [Fact]
        public void Should_Pass_Through_IAttributeInfo_When_Fact_Attribute_Type_Is_Not_Provided()
        {
            var reflectionAttributeInfo = GetReflectionAttributeInfo_DiscovererType(FakeDiscoverer.Type);

            Discover_No_Skip(reflectionAttributeInfo, reflectionAttributeInfo);
        }

        class AFactAttribute : FactAttribute
        {
            public static Type Type => typeof(AFactAttribute);
        }

        [Fact]
        public void Should_Create_And_Pass_Through_ReflectionAttributeInfo_When_Fact_Attribute_Type_Is_Provided()
        {
            var factAttributeArgs = new object[] { };
            var reflectionAttributeInfo = GetReflectionAttributeInfo_FactType(AFactAttribute.Type, factAttributeArgs);
            var customAttributeDataWrapper = new Mock<ICustomAttributeDataWrapper>().Object;
            
            var mockReflectionHelper = new Mock<IReflectionHelper>();
            mockReflectionHelper.Setup(rh => rh.CustomAttributeDataRepresentingFactAttribute(AFactAttribute.Type, factAttributeArgs)).Returns(customAttributeDataWrapper);
            
            var reflectionAttributeInfoForFactAttribute = new Mock<IReflectionAttributeInfo>().Object;
            var mockReflectionAttributeInfoFactory = new Mock<IReflectionAttributeInfoFactory>();
            mockReflectionAttributeInfoFactory.Setup(f => f.Create(customAttributeDataWrapper)).Returns(reflectionAttributeInfoForFactAttribute);
            
            Discover_No_Skip(reflectionAttributeInfo, reflectionAttributeInfoForFactAttribute, conditionalFactDiscoverer =>
            {
                conditionalFactDiscoverer.ReflectionHelper = mockReflectionHelper.Object;
                conditionalFactDiscoverer.ReflectionAttributeInfoFactory = mockReflectionAttributeInfoFactory.Object;
            });
        }
    }
    
    
}
