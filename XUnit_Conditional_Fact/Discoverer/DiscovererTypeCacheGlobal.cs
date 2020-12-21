using System;
using System.Collections.Generic;

namespace XUnit_Conditional_Fact
{
    internal static class DiscovererTypeCacheGlobal
    {
        private static Dictionary<Type, Type> dictionary = new Dictionary<Type, Type>();
        public static void Add(Type factAttributeType, Type discovererType)
        {
            dictionary.Add(factAttributeType, discovererType);
        }

        public static bool TryGetValue(Type factAttributeType, out Type discovererType)
        {
            return dictionary.TryGetValue(factAttributeType, out discovererType);
        }
    }
}
