using System;
using Xunit;
using Xunit.Sdk;

namespace XUnit_Conditional_Fact
{
    [AttributeUsage(AttributeTargets.Method)]
    [XunitTestCaseDiscoverer("XUnit_Conditional_Fact." + nameof(ConditionalFactDiscoverer), "XUnit_Conditional_Fact")]
    public class ConditionalFact : FactAttribute {
        internal Type DiscovererType { get; }
        internal Type SkipLogicType { get; }
        internal Type DiscovererFactAttributeType { get; }
        internal object[] DiscovererFactAttributeCtorArgs { get; }

        /// <summary>
        ///     The most common constructor.
        /// </summary>
        /// <param name="discovererFactAttributeType"></param>
        /// <param name="factAttributeCtorArgs"></param>
        /// <param name="skipLogicType"></param>
        public ConditionalFact(Type discovererFactAttributeType, object[] factAttributeCtorArgs, Type skipLogicType = null)
        {
            DiscovererFactAttributeType = discovererFactAttributeType;
            DiscovererFactAttributeCtorArgs = factAttributeCtorArgs;
            SkipLogicType = skipLogicType;
        }

        /*
            XUnit discovery - see XunitTestFrameworkDiscoverer.FindTestsForMethod

            XunitTestCaseDiscovererAttribute on the FactAttribute derivation provides the type ( and its assembly ) of the associated 
            IXunitTestCaseDiscoverer.  For TheoryAttribute and FactAttribute the assembly name is not a true assembly name.

            If the SerializationHelper.GetType method was not on internal class then could have used that and there would have
            been no need to provide the discoverer type. Could have used reflection ....
        
        */
         

        /// <summary>
        ///     This is for Theory and Fact where the XunitTestCaseDiscoverer assemblyName is not a true assembly name 
        /// </summary>
        /// <param name="discovererType"></param>
        /// <param name="skipLogicType"></param>
        public ConditionalFact(Type discovererType,Type skipLogicType = null)
        {
            DiscovererType = discovererType;
            SkipLogicType = skipLogicType;
        }
        
        /// <summary>
        ///     This is for future XUnit discoverers where the XunitTestCaseDiscoverer assemblyName is not a true assembly name
        ///     and the discoverer requires the Fact attribute derivation
        /// </summary>
        /// <param name="discovererType"></param>
        /// <param name="discovererFactAttributeType"></param>
        /// <param name="factAttributeCtorArgs"></param>
        /// <param name="skipLogicType"></param>
        public ConditionalFact(Type discovererType, Type discovererFactAttributeType, object[] factAttributeCtorArgs, Type skipLogicType = null)
        {
            DiscovererType = discovererType;
            SkipLogicType = skipLogicType;
            DiscovererFactAttributeType = discovererFactAttributeType;
            DiscovererFactAttributeCtorArgs = factAttributeCtorArgs;
        }
    }
}
