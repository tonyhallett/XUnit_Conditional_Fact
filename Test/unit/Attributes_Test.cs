using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;
using XUnit_Conditional_Fact;

namespace Test.unit
{
    public class Attributes_Test
    {
        [Fact]
        public void FactConditional_Should_Apply_To_FactDiscoverer()
        {
            var factConditional = new FactConditional();
            Assert.Same(typeof(FactDiscoverer), factConditional.DiscovererType);
        }

        [Fact]
        public void FactConditional_Should_Allow_Custom_SkipLogic()
        {
            var factConditional = new FactConditional(CustomSkipLogic.Type);
            Assert.Same(CustomSkipLogic.Type, factConditional.SkipLogicType);
            Assert.Same(typeof(FactDiscoverer), factConditional.DiscovererType);
        }

        [Fact]
        public void TheoryConditional_Should_Apply_To_FactDiscoverer()
        {
            var theoryConditional = new TheoryConditional();
            Assert.Same(typeof(TheoryDiscoverer), theoryConditional.DiscovererType);

        }

        [Fact]
        public void TheoryConditional_Should_Allow_Custom_SkipLogic()
        {
            var theoryConditional = new TheoryConditional(CustomSkipLogic.Type);
            Assert.Same(CustomSkipLogic.Type, theoryConditional.SkipLogicType);
            Assert.Same(typeof(TheoryDiscoverer), theoryConditional.DiscovererType);
        }

        class CustomSkipLogic
        {
            public static Type Type => typeof(CustomSkipLogic);
        }

        class Discoverer {
            public static Type Type => typeof(Discoverer);
        }
        class DiscovererFactAttribute : FactAttribute
        {
            public DiscovererFactAttribute(string arg) { }
            public static Type Type => typeof(DiscovererFactAttribute);
        }

        [Fact]
        public void Conditional_Fact_Ctor_With_Most_Args_Sets_Properties_Properly()
        {
            var factAttributeCtorArgs = new object[] { "1" };
            var conditionalFact = new ConditionalFact(Discoverer.Type, DiscovererFactAttribute.Type, factAttributeCtorArgs, CustomSkipLogic.Type);
            Assert.Same(Discoverer.Type, conditionalFact.DiscovererType);
            Assert.Same(DiscovererFactAttribute.Type, conditionalFact.DiscovererFactAttributeType);
            Assert.Same(factAttributeCtorArgs, conditionalFact.DiscovererFactAttributeCtorArgs);
            Assert.Same(CustomSkipLogic.Type, conditionalFact.SkipLogicType);

        }
    }
}
