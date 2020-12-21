using Xunit.Abstractions;

namespace XUnit_Conditional_Fact
{
    internal interface IReflectionAttributeInfoFactory
    {
        IReflectionAttributeInfo Create(ICustomAttributeDataWrapper customAttributeDataWrapper);
    }
    
}
