using System;
using System.Collections.Generic;
using System.Text;
using XUnit_Conditional_Fact;

namespace Test
{
    class TestCondition:Attribute, ITestCondition
    {
        public TestCondition()
        {
        }
        public TestCondition(string skipReason)
        {
            SkipReason = skipReason;
        }
        public string SkipReason { get; }
    }

    class OtherTestCondition : TestCondition
    {

    }
}
