using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Sdk;

namespace XUnit_Conditional_Fact
{
    public class FactConditional:ConditionalFact
    {
        public FactConditional() : base(typeof(FactDiscoverer)) { }
        public FactConditional(Type skipLogicType) : base(typeof(FactDiscoverer), skipLogicType) { }
    }
}
