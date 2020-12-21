using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using XUnit_Conditional_Fact;

namespace Test.unit
{
    class AttributeReflectionAttributeInfo: IReflectionAttributeInfo
    {
        private readonly Attribute attribute;

        public static IEnumerable<IReflectionAttributeInfo> Single(string skipReason)
        {
            return new IReflectionAttributeInfo[] { new AttributeReflectionAttributeInfo(skipReason) };
        }

        public static IEnumerable<IReflectionAttributeInfo> SingleConditionMet()
        {
            return new IReflectionAttributeInfo[] { new AttributeReflectionAttributeInfo(null) };
        }


        public AttributeReflectionAttributeInfo(string skipReason)
        {
            this.attribute = new TestCondition(skipReason);
        }

        public Attribute Attribute => attribute;

        public IEnumerable<object> GetConstructorArguments()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName)
        {
            throw new NotImplementedException();
        }

        public TValue GetNamedArgument<TValue>(string argumentName)
        {
            throw new NotImplementedException();
        }
    }
    public class TestConditionAttributeIsMetSkipLogic_Test
    {
        private string ITestConditionFullyQualifiedName = typeof(ITestCondition).AssemblyQualifiedName;
        [Fact]
        public void Should_Skip_If_TestCondition_Attribute_From_Assembly_Is_Not_Met()
        {
            var skipReason = "Assembly skip";
            TestConditionLogic(Array.Empty<AttributeReflectionAttributeInfo>(), Array.Empty<AttributeReflectionAttributeInfo>(), AttributeReflectionAttributeInfo.Single(skipReason), skipReason);
        }

        private void TestConditionLogic(IEnumerable<IAttributeInfo> methodCustomAttributes, IEnumerable<IAttributeInfo> classCustomAttributes, IEnumerable<IAttributeInfo> assemblyCustomAttributes,string expectedSkipReason)
        {
            var skipLogic = new TestConditionAttributeIsMetSkipLogic();
            var mockedTestMethod = Mock.Of<ITestMethod>(tm =>
                tm.Method.GetCustomAttributes(ITestConditionFullyQualifiedName) == methodCustomAttributes &&
                tm.TestClass.Class.GetCustomAttributes(ITestConditionFullyQualifiedName) == classCustomAttributes &&
                tm.TestClass.TestCollection.TestAssembly.Assembly.GetCustomAttributes(ITestConditionFullyQualifiedName) == assemblyCustomAttributes
            );
            var skipReason = skipLogic.GetSkipReason(mockedTestMethod);
            Assert.Same(expectedSkipReason, skipReason);
        }

        [Fact]
        public void Should_Skip_If_TestCondition_Attribute_From_Class_Is_Not_Met()
        {
            var skipReason = "Class skip";
            TestConditionLogic(Array.Empty<AttributeReflectionAttributeInfo>(),  AttributeReflectionAttributeInfo.Single(skipReason), Array.Empty<AttributeReflectionAttributeInfo>(), skipReason);
        }
    

        [Fact]
        public void Should_Skip_If_TestCondition_Attribute_From_Method_Is_Not_Met()
        {
            var skipReason = "Method skip";
            TestConditionLogic(AttributeReflectionAttributeInfo.Single(skipReason), Array.Empty<AttributeReflectionAttributeInfo>(), Array.Empty<AttributeReflectionAttributeInfo>(), skipReason);
        }

        [Fact]
        public void ITestCondition_Attribute_On_Method_Takes_Precedence()
        {
            var skipReason = "Precedence";
            TestConditionLogic(new List<AttributeReflectionAttributeInfo> { new AttributeReflectionAttributeInfo(skipReason), new AttributeReflectionAttributeInfo("Other method skip") }, AttributeReflectionAttributeInfo.Single("Class skip"), AttributeReflectionAttributeInfo.Single("Assembly skip"), skipReason);
        }

        [Fact]
        public void ITestCondition_Attribute_On_Class_Takes_Precedence_Over_Assembly()
        {
            var skipReason = "Precedence";
            TestConditionLogic(Array.Empty<AttributeReflectionAttributeInfo>(),new List<AttributeReflectionAttributeInfo> { new AttributeReflectionAttributeInfo(skipReason), new AttributeReflectionAttributeInfo("Other class skip") }, AttributeReflectionAttributeInfo.Single("Assembly skip"), skipReason);
        }

        [Fact]
        public void Should_Not_Skip_If_All_TestCondition_Attributes_Are_Met()
        {
            TestConditionLogic(AttributeReflectionAttributeInfo.SingleConditionMet(), AttributeReflectionAttributeInfo.SingleConditionMet(), AttributeReflectionAttributeInfo.SingleConditionMet(), null);
        }

    }
}
