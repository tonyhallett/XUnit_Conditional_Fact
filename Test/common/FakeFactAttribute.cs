using System;
using Xunit;

namespace Test
{
    class FakeFactAttribute : FactAttribute
    {
        public FakeFactAttribute(object arg) { }
        public static Type Type => typeof(FakeFactAttribute);
    }

}
