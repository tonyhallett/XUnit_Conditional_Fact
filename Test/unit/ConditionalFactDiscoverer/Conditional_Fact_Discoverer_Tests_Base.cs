using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;
using XUnit_Conditional_Fact;

namespace Test
{
    public class Conditional_Fact_Discoverer_Tests_Base
    {
#pragma warning disable xUnit3000 // Test case classes must derive directly or indirectly from Xunit.LongLivedMarshalByRefObject
        [ExcludeFromCodeCoverage]
        public class FakeTestCase : IXunitTestCase
#pragma warning restore xUnit3000 // Test case classes must derive directly or indirectly from Xunit.LongLivedMarshalByRefObject
        {
            public Exception InitializationException => throw new NotImplementedException();

            public IMethodInfo Method => throw new NotImplementedException();

            public int Timeout => throw new NotImplementedException();

            public string DisplayName => throw new NotImplementedException();

            public string SkipReason => throw new NotImplementedException();

            public ISourceInformation SourceInformation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public ITestMethod TestMethod => throw new NotImplementedException();

            public object[] TestMethodArguments => throw new NotImplementedException();

            public Dictionary<string, List<string>> Traits => throw new NotImplementedException();

            public string UniqueID => throw new NotImplementedException();

            public void Deserialize(IXunitSerializationInfo info)
            {
                throw new NotImplementedException();
            }

            public Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
            {
                throw new NotImplementedException();
            }

            public void Serialize(IXunitSerializationInfo info)
            {
                throw new NotImplementedException();
            }
        }

        protected ITestMethod mockedTestMethod = new Mock<ITestMethod>().Object;
        protected IMessageSink mockedMessageSink = new Mock<IMessageSink>().Object;
        protected ITestFrameworkDiscoveryOptions mockedTestFrameworkDiscoveryOptions = new Mock<ITestFrameworkDiscoveryOptions>().Object;
        protected IReflectionAttributeInfo GetReflectionAttributeInfo_DiscovererType(Type discovererType = null, Type skipLogicType = null)
        {
            var mockReflectionAttributeInfo = new Mock<IReflectionAttributeInfo>();
            mockReflectionAttributeInfo.SetupGet(ai => ai.Attribute).Returns(new ConditionalFact(discovererType, skipLogicType));
            return mockReflectionAttributeInfo.Object;
        }

        protected IReflectionAttributeInfo GetReflectionAttributeInfo_FactType(Type discovererFactAttributeType, object[] factAttributeCtorArgs, Type skipLogicType = null)
        {
            var mockReflectionAttributeInfo = new Mock<IReflectionAttributeInfo>();
            mockReflectionAttributeInfo.SetupGet(ai => ai.Attribute).Returns(new ConditionalFact(discovererFactAttributeType, factAttributeCtorArgs, skipLogicType));
            return mockReflectionAttributeInfo.Object;
        }

        internal void SetUpSkipLogic(ConditionalFactDiscoverer conditionalFactDiscoverer, Type getOrCreateType, string skipReason)
        {
            var mockSkipLogicFactory = new Mock<ISkipLogicFactory>();
            var mockSkipLogic = new Mock<ISkipLogic>();
            mockSkipLogic.Setup(l => l.GetSkipReason(mockedTestMethod)).Returns(skipReason);
            mockSkipLogicFactory.Setup(slf => slf.CreateOrDefault(getOrCreateType)).Returns(mockSkipLogic.Object);
            conditionalFactDiscoverer.SkipLogicFactory = mockSkipLogicFactory.Object;
        }
    }
    
}
