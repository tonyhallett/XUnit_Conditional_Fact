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

namespace Test.unit
{
    public class XunitTestCaseDiscovererProvider_Tests
    {
        class Discoverer : IXunitTestCaseDiscoverer
        {
            public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
            {
                throw new NotImplementedException();
            }
        }
        [Fact]
        public void Should_Instantiate_The_Discoverer_Type_From_The_ConditionalFact()
        {
            var mockedInstantiatedDiscoverer = new Discoverer();
            var messageSink = new Mock<IMessageSink>().Object;
            var provider = new XunitTestCaseDiscovererProvider();
            var mockReflectionHelper = new Mock<IReflectionHelper>();
            mockReflectionHelper.Setup(rh => rh.CreateInstance<IXunitTestCaseDiscoverer>(FakeDiscoverer.Type, It.Is<object[]>(args => args.Length == 1 && args[0] == messageSink))).Returns(mockedInstantiatedDiscoverer);
            var conditionalFactWithDiscovererType = new ConditionalFact(FakeDiscoverer.Type);
            provider.ReflectionHelper = mockReflectionHelper.Object;
            var discoverer = provider.Provide(conditionalFactWithDiscovererType, messageSink);
            Assert.Same(mockedInstantiatedDiscoverer, discoverer);
        }

        [Fact]
        public void Should_Instantiate_From_The_Type_In_The_Cache_If_Present()
        {
            var provider = new XunitTestCaseDiscovererProvider();

            var mockedInstantiatedDiscoverer = new Discoverer();
            var messageSink = new Mock<IMessageSink>().Object;
            var conditionalFactWithAttributeType = new ConditionalFact(FakeFactAttribute.Type, new object[] { });

            var mockDiscovererTypeCache = new Mock<IDiscovererTypeCache>();
            var discovererTypeFromCache = FakeDiscoverer.Type;
            mockDiscovererTypeCache.Setup(c => c.TryGetValue(FakeFactAttribute.Type, out discovererTypeFromCache)).Returns(true);
            provider.DiscovererTypeCache = mockDiscovererTypeCache.Object;

            var mockReflectionHelper = new Mock<IReflectionHelper>();
            mockReflectionHelper.Setup(rh => rh.CreateInstance<IXunitTestCaseDiscoverer>(discovererTypeFromCache, It.Is<object[]>(args => args.Length == 1 && args[0] == messageSink))).Returns(mockedInstantiatedDiscoverer);
            
            provider.ReflectionHelper = mockReflectionHelper.Object;
            var discoverer = provider.Provide(conditionalFactWithAttributeType, messageSink);
            Assert.Same(mockedInstantiatedDiscoverer, discoverer);
        }

        [Fact]
        public void Should_Instantiate_The_Discoverer_Specified_On_The_Fact_Attribute_And_Add_To_Cache_When_Not_Cached()
        {
            var provider = new XunitTestCaseDiscovererProvider();

            var mockedInstantiatedDiscoverer = new Discoverer();
            var messageSink = new Mock<IMessageSink>().Object;
            var conditionalFactWithAttributeType = new ConditionalFact(FakeFactAttribute.Type, new object[] { });

            var mockDiscovererTypeCache = new Mock<IDiscovererTypeCache>();
            Type discovererTypeFromCache = null;
            mockDiscovererTypeCache.Setup(c => c.TryGetValue(FakeFactAttribute.Type, out discovererTypeFromCache)).Returns(false);
            provider.DiscovererTypeCache = mockDiscovererTypeCache.Object;
            var discovererTypeName = "discoverer";
            var discovererAssemblyName = "assemblyName";
            var mockReflectionHelper = new Mock<IReflectionHelper>();
            mockReflectionHelper.Setup(rh => rh.GetDiscovererTypeNameAndAssemblyName(FakeFactAttribute.Type)).Returns((discovererTypeName, discovererAssemblyName));
            mockReflectionHelper.Setup(rh => rh.GetType(discovererTypeName, discovererAssemblyName)).Returns(FakeDiscoverer.Type);
            mockReflectionHelper.Setup(rh => rh.CreateInstance<IXunitTestCaseDiscoverer>(FakeDiscoverer.Type, It.Is<object[]>(args => args.Length == 1 && args[0] == messageSink))).Returns(mockedInstantiatedDiscoverer);

            provider.ReflectionHelper = mockReflectionHelper.Object;
            var discoverer = provider.Provide(conditionalFactWithAttributeType, messageSink);
            mockDiscovererTypeCache.Verify(c => c.Add(FakeFactAttribute.Type, FakeDiscoverer.Type));
            Assert.Same(mockedInstantiatedDiscoverer, discoverer);
        }
    }
}
