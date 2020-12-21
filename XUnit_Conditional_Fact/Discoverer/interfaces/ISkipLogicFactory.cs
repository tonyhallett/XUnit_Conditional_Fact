using System;

namespace XUnit_Conditional_Fact
{
    internal interface ISkipLogicFactory
    {
        ISkipLogic CreateOrDefault(Type providedSkipLogic);
    }
    
}
