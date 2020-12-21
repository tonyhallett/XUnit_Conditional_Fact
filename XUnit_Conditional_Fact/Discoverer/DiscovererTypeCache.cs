using System;
using System.Collections.Generic;

namespace XUnit_Conditional_Fact
{
    internal class DiscovererTypeCache : IDiscovererTypeCache
    {
        private Dictionary<Type, Type> dictionary = new Dictionary<Type, Type>();
        public void Add(Type factAttributeType, Type discovererType)
        {
            DiscovererTypeCacheGlobal.Add(factAttributeType, discovererType);
        }

        public bool TryGetValue(Type factAttributeType, out Type discovererType)
        {
            return DiscovererTypeCacheGlobal.TryGetValue(factAttributeType, out discovererType);
        }
    }
}
