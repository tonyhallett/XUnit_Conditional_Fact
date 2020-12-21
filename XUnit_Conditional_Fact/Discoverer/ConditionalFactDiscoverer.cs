using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace XUnit_Conditional_Fact
{
    internal class ConditionalFactDiscoverer : IXunitTestCaseDiscoverer
    {
        private readonly IMessageSink messageSink;
        private ITestFrameworkDiscoveryOptions discoveryOptions;

        public ConditionalFactDiscoverer(IMessageSink messageSink)
        {
            this.messageSink = messageSink;
        }
        private IXunitTestCase GetSkippedTestCase(ITestMethod testMethod,Type skipLogicType)
        {
            ISkipLogic skipLogic = SkipLogicFactory.CreateOrDefault(skipLogicType);
            var skipReason = skipLogic.GetSkipReason(testMethod);
            if(skipReason == null)
            {
                return null;
            }
            return TestCaseFactory.CreateSkip(skipReason, messageSink, discoveryOptions, testMethod);
        }
        internal IXunitTestCaseDiscovererProvider DiscovererProvider { get; set; } = new XunitTestCaseDiscovererProvider();
        internal ISkipLogicFactory SkipLogicFactory { get; set; } = new SkipLogicFactory();
        internal IReflectionHelper ReflectionHelper { get; set; } = new ReflectionHelper();
        internal ITestCaseFactory TestCaseFactory { get; set; } = new TestCaseFactory();
        internal IReflectionAttributeInfoFactory ReflectionAttributeInfoFactory { get; set; } = new ReflectionAttributeInfoFactory();
        
        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            try
            {
                this.discoveryOptions = discoveryOptions;
                /*
                    note that GetNamedArgument fails if argument was not passed
                    Fails on public FactConditional() : base(typeof(FactDiscoverer)) { }
                    so have no choice but go to reflection
                */
                var conditionalFact = (factAttribute as IReflectionAttributeInfo).Attribute as ConditionalFact;

                var skippedTestCase = GetSkippedTestCase(testMethod, conditionalFact.SkipLogicType);
                if (skippedTestCase != null)
                {
                    return new IXunitTestCase[] { skippedTestCase };
                }

                var discovererFactAttributeType = conditionalFact.DiscovererFactAttributeType;

                // TheoryDiscoverer will check for Skip - sufficient to use that of ConditionalFactAttribute
                IAttributeInfo wrappedFactAttribute = factAttribute;
                if (discovererFactAttributeType != null)
                {
                    var customAttributeData = ReflectionHelper.CustomAttributeDataRepresentingFactAttribute(discovererFactAttributeType, conditionalFact.DiscovererFactAttributeCtorArgs);
                    wrappedFactAttribute = ReflectionAttributeInfoFactory.Create(customAttributeData);
                }

                var wrappedDiscoverer = DiscovererProvider.Provide(conditionalFact, messageSink);
                return wrappedDiscoverer.Discover(discoveryOptions, testMethod, wrappedFactAttribute);

            }catch(Exception exc)
            {
                return new IXunitTestCase[] { TestCaseFactory.CreateError(exc.Message, messageSink, discoveryOptions, testMethod) };
            }
        }
    }
}
