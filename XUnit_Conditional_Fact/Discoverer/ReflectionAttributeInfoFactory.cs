using Xunit.Abstractions;
using Xunit.Sdk;

namespace XUnit_Conditional_Fact
{
    internal class ReflectionAttributeInfoFactory : IReflectionAttributeInfoFactory
    {
        public IReflectionAttributeInfo Create(ICustomAttributeDataWrapper customAttributeDataWrapper)
        {
            return new ReflectionAttributeInfo(customAttributeDataWrapper.Wrapped);
        }
    }
    
}
