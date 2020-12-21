using System.Linq;
using Xunit.Abstractions;

namespace XUnit_Conditional_Fact
{
    internal class TestConditionAttributeIsMetSkipLogic : ISkipLogic
    {
        public string GetSkipReason(ITestMethod testMethod)
        {
            var testClass = testMethod.TestClass.Class;
            var assembly = testMethod.TestClass.TestCollection.TestAssembly.Assembly;

            var conditionAttributes = testMethod.Method
                .GetCustomAttributes(typeof(ITestCondition).AssemblyQualifiedName)
                .Concat(testClass.GetCustomAttributes(typeof(ITestCondition).AssemblyQualifiedName))
                .Concat(assembly.GetCustomAttributes(typeof(ITestCondition).AssemblyQualifiedName))
                .OfType<IReflectionAttributeInfo>()
                .Select(attributeInfo => attributeInfo.Attribute);

            foreach (ITestCondition condition in conditionAttributes)
            {
                if (condition.SkipReason != null)
                {
                    return condition.SkipReason;
                }
            }

            return null;
        }
    }
    
}
