using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnit_Conditional_Fact
{
    internal interface IDiscovererTypeCache
    {
        bool TryGetValue(Type factAttributeType, out Type discovererType);
        void Add(Type factAttributeType, Type discovererType);
    }
}
