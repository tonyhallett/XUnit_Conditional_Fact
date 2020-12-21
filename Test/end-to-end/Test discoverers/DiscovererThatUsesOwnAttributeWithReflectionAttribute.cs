using Xunit;
using Xunit.Sdk;

namespace Test
{
    [XunitTestCaseDiscoverer("Test." + nameof(DiscovererThatUsesOwnAttributeWithReflection), "Test")]
    public class DiscovererThatUsesOwnAttributeWithReflectionAttribute : FactAttribute
    {
        private readonly string someArg;

        public DiscovererThatUsesOwnAttributeWithReflectionAttribute(string someArg)
        {
            this.someArg = someArg;
        }
        public string SomeMethod()
        {
            return someArg;
        }
    }
}
