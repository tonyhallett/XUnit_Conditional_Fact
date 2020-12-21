using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using XUnit_Conditional_Fact;

namespace Test
{
    public class FakeSkipLogic : ISkipLogic
    {
        public static Type Type => typeof(FakeSkipLogic);

        [ExcludeFromCodeCoverage]
        public string GetSkipReason(ITestMethod testMethod)
        {
            throw new NotImplementedException();
        }
    }


}
