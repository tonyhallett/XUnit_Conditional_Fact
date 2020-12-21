using System;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace XUnit_Conditional_Fact
{
    internal class XunitTestCaseDiscovererProvider : IXunitTestCaseDiscovererProvider
    {
        internal IReflectionHelper ReflectionHelper { get; set; } = new ReflectionHelper();
        internal IDiscovererTypeCache DiscovererTypeCache { get; set; } = new DiscovererTypeCache();
        public IXunitTestCaseDiscoverer Provide(ConditionalFact conditionalFact, IMessageSink messageSink)
        {
            Type discovererType = null;
            if (conditionalFact.DiscovererType != null)
            {
                discovererType = conditionalFact.DiscovererType;
            }
            else
            {
                var discovererLoaded = DiscovererTypeCache.TryGetValue(conditionalFact.DiscovererFactAttributeType, out discovererType);
                if (!discovererLoaded)
                {
                    var (typeName, assemblyName) = ReflectionHelper.GetDiscovererTypeNameAndAssemblyName(conditionalFact.DiscovererFactAttributeType);
                    discovererType = ReflectionHelper.GetType(typeName, assemblyName);
                    DiscovererTypeCache.Add(conditionalFact.DiscovererFactAttributeType, discovererType);
                }
                
            }

            return ReflectionHelper.CreateInstance<IXunitTestCaseDiscoverer>(discovererType, new object[] { messageSink });
        }
    }
}
