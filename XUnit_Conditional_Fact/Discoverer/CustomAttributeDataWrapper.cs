using System.Reflection;

namespace XUnit_Conditional_Fact
{
    internal class CustomAttributeDataWrapper : ICustomAttributeDataWrapper
    {
        private readonly CustomAttributeData wrapped;

        public CustomAttributeDataWrapper(CustomAttributeData wrapped)
        {
            this.wrapped = wrapped;
        }

        public CustomAttributeData Wrapped => wrapped;
    }
    
}
