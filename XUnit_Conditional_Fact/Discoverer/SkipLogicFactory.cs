using System;

namespace XUnit_Conditional_Fact
{
    internal class SkipLogicFactory : ISkipLogicFactory
    {
        private readonly IReflectionHelper typeCreator;

        public SkipLogicFactory():this(new ReflectionHelper()) { }
        internal SkipLogicFactory(IReflectionHelper typeCreator)
        {
            this.typeCreator = typeCreator;
        }
        public ISkipLogic CreateOrDefault(Type providedSkipLogicType)
        {
            return providedSkipLogicType == null ? new TestConditionAttributeIsMetSkipLogic() : typeCreator.CreateInstance<ISkipLogic>(providedSkipLogicType,new object[] { });
        }
    }
    
}
