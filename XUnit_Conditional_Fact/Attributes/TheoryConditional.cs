using System;
using Xunit.Sdk;

namespace XUnit_Conditional_Fact
{
    public class TheoryConditional : ConditionalFact
    {
        public TheoryConditional() : base(typeof(TheoryDiscoverer)) { }
        public TheoryConditional(Type skipLogicType) : base(typeof(TheoryDiscoverer), skipLogicType) { }
    }
}
