using System;

namespace XUnit_Conditional_Fact
{
    internal interface IReflectionHelper
    {
        T CreateInstance<T>(Type type,object[] args);
        (string typeName, string assemblyName) GetDiscovererTypeNameAndAssemblyName(Type factAttributeType);
        Type GetType(string typeName, string assemblyName);
        ICustomAttributeDataWrapper CustomAttributeDataRepresentingFactAttribute(Type factAttributeType, object[] discovererFactAttributeCtorArgs);
        
    }
    
}
