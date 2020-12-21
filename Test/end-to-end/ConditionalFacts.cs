using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using XUnit_Conditional_Fact;

namespace Test
{
#if NET472
    public class ConditionalFacts : AcceptanceTestV2
    {
        [Fact]
        public void FactConditional_Should_Skip_When_Any_Condition_Is_Not_Met()
        {
            var msgs = Run<ITestSkipped>(typeof(FactConditionalSkip));
            
            var expectedTestDisplayName = $"{nameof(Test)}.{nameof(ConditionalFacts)}+{nameof(FactConditionalSkip)}.{nameof(FactConditionalSkip.Test_Should_Not_Run)}";
            Assert.Collection(msgs,
                msg => Assert.Equal(expectedTestDisplayName, msg.Test.DisplayName)
            );
            Assert.Equal("Skipping", msgs[0].Reason);
        }
        
        public class FactConditionalSkip
        {
            [FactConditional()]
            [TestCondition("Skipping")]
            [OtherTestCondition]
            [Trait("Do not run directly","")]
            public void Test_Should_Not_Run()
            {
            }
        }

        [Fact]
        public void FactConditional_Should_Not_Skip_And_Pass_Through_When_All_Conditions_Are_Met()
        {
            var msgs = Run<ITestPassed>(typeof(FactConditionalNoSkip));
            var expectedTestDisplayName = $"{nameof(Test)}.{nameof(ConditionalFacts)}+{nameof(FactConditionalNoSkip)}.{nameof(FactConditionalNoSkip.Test_Should_Run)}";
            Assert.Collection(msgs,
                msg => Assert.Equal(expectedTestDisplayName, msg.Test.DisplayName)
            );
        }
        
        public class FactConditionalNoSkip
        {
            [FactConditional]
            [TestCondition]
            [Trait("Do not run directly", "")]
            public void Test_Should_Run()
            {
            }


        }

        [Fact]
        public void TheoryConditional_Should_Not_Skip_And_Pass_Through_When_All_Conditions_Are_Met()
        {
            string ExpectedTestCaseDisplayName(bool parameterized)
            {
                var parameterizedString = parameterized ? "True":"False";
                return $"{nameof(Test)}.{nameof(ConditionalFacts)}+{nameof(TheoryConditionalNoSkip)}.{nameof(TheoryConditionalNoSkip.Theory_Should_Run)}(parameterized: {parameterizedString})";
            }
            var msgs = Run<ITestPassed>(typeof(TheoryConditionalNoSkip));
            
            var expectedTestDisplayName = $"{nameof(Test)}.{nameof(ConditionalFacts)}+{nameof(TheoryConditionalNoSkip)}.{nameof(TheoryConditionalNoSkip.Theory_Should_Run)}";
            Assert.Collection(msgs,
                msg => Assert.Equal(ExpectedTestCaseDisplayName(false), msg.Test.DisplayName),
                msg => Assert.Equal(ExpectedTestCaseDisplayName(true), msg.Test.DisplayName)
            );
        }

        public class TheoryConditionalNoSkip
        {
            [TheoryConditional]
            [InlineData(true)]
            [InlineData(false)]
            [TestCondition]
            [Trait("Do not run directly", "")]
            public void Theory_Should_Run(bool parameterized)
            {
            }
        }

        [Fact]
        public void TheoryConditional_Should_Skip_When_Any_Conditions_Is_Not_Met()
        {
            var msgs = Run<ITestSkipped>(typeof(TheoryConditionalSkip));

            var expectedTestDisplayName = $"{nameof(Test)}.{nameof(ConditionalFacts)}+{nameof(TheoryConditionalSkip)}.{nameof(TheoryConditionalSkip.Theory_Should_Not_Run)}";
            Assert.Collection(msgs,
                msg => Assert.Equal(expectedTestDisplayName, msg.Test.DisplayName)
            );
            Assert.Equal("Skip", msgs[0].Reason);
        }

        public class TheoryConditionalSkip
        {
            [TheoryConditional]
            [InlineData(true)]
            [InlineData(false)]
            [TestCondition("Skip")]
            [OtherTestCondition]
            [Trait("Do not run directly", "")]
            public void Theory_Should_Not_Run(bool parameterized)
            {
            }
        }

        [Fact]
        public void Custom_Discoverer_Should_Be_Called_With_Its_Fact_Attribute_When_Provided_And_When_Not_Skip()
        {
            // skipped because the custom discoverer skips
            var msgs = Run<ITestSkipped>(typeof(CustomNoSkip));

            var expectedTestDisplayName = $"{nameof(Test)}.{nameof(ConditionalFacts)}+{nameof(CustomNoSkip)}.{nameof(CustomNoSkip.Wrapped_Requires_Own_Fact_Attribute)}";
            Assert.Collection(msgs,
                msg => Assert.Equal(expectedTestDisplayName, msg.Test.DisplayName)
            );
            Assert.Equal("Custom Discoverer Attribute Ctor Arg Used For Skip through attribute type", msgs[0].Reason);
        }

        public class CustomNoSkip
        {
            [ConditionalFact(typeof(DiscovererThatUsesOwnAttributeWithReflectionAttribute), new object[] { "Custom Discoverer Attribute Ctor Arg Used For Skip through attribute type" })]
            [Trait("Do not run directly", "")]
            public void Wrapped_Requires_Own_Fact_Attribute()
            {

            }
        }

        [Fact]
        public void Should_Instantiate_And_Call_Custom_SkipLogic()
        {
            var msgs = Run<ITestSkipped>(typeof(CustomSkipLogic));

            var expectedTestDisplayName = $"{nameof(Test)}.{nameof(ConditionalFacts)}+{nameof(CustomSkipLogic)}.{nameof(CustomSkipLogic.Skip_This)}";
            Assert.Collection(msgs,
                msg => Assert.Equal(expectedTestDisplayName, msg.Test.DisplayName)
            );
            Assert.Equal(CustomSkipLogicSkipsMethodsStartingWithSkip.CustomSkipLogicSkipReason, msgs[0].Reason);
        }

        class CustomSkipLogicSkipsMethodsStartingWithSkip : ISkipLogic
        {
            public static readonly string CustomSkipLogicSkipReason = "Skippable method";
            public string GetSkipReason(ITestMethod testMethod)
            {
                if (testMethod.Method.Name.StartsWith("Skip"))
                {
                    return CustomSkipLogicSkipReason;
                }
                return null;
            }
        }

        public class CustomSkipLogic
        {
            // Could have used [FactConditional(typeof(CustomSkipLogicSkipsMethodsStartingWithSkip))] 
            [ConditionalFact(typeof(FactDiscoverer),typeof(CustomSkipLogicSkipsMethodsStartingWithSkip))]
            [Trait("Do not run directly", "")]
            public void Skip_This()
            {

            }
        }

        [Fact]
        public void Should_Pass_Through_When_Custom_SkipLogic_Does_Not_Skip()
        {
            var msgs = Run<ITestPassed>(typeof(CustomSkipLogicNoSkip));

            var expectedTestDisplayName = $"{nameof(Test)}.{nameof(ConditionalFacts)}+{nameof(CustomSkipLogicNoSkip)}.{nameof(CustomSkipLogicNoSkip.Do_Not_Skip_This)}";
            Assert.Collection(msgs,
                msg => Assert.Equal(expectedTestDisplayName, msg.Test.DisplayName)
            );
        }
        public class CustomSkipLogicNoSkip
        {
            [ConditionalFact(typeof(FactDiscoverer), typeof(CustomSkipLogicSkipsMethodsStartingWithSkip))]
            [Trait("Do not run directly", "")]
            public void Do_Not_Skip_This()
            {

            }
        }

        [Fact]
        public void Should_Fail_With_Exception_Message_If_Exception_In_Discover()
        {
            var msgs = Run<ITestFailed>(typeof(Failing));
            var expectedTestDisplayName = $"{nameof(Test)}.{nameof(ConditionalFacts)}+{nameof(Failing)}.{nameof(Failing.Will_Fail)}";
            Assert.Collection(msgs,
                msg => Assert.Equal(expectedTestDisplayName, msg.Test.DisplayName)
            );
            Assert.Equal("Constructor on type 'Test.ConditionalFacts+NotADiscoverer' not found.", msgs[0].Messages[0]);
        }

        class NotADiscoverer
        {

        }

        public class Failing {

            [ConditionalFact(typeof(NotADiscoverer))]
            [Trait("Do not run directly", "")]
            public void Will_Fail() // this should not be run directly 
            {

            }
        
        }

    }
#endif
}
