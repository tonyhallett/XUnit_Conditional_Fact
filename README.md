Make a [Fact] or [Theory] test skippable by using [FactConditional] or [TheoryConditional] instead.

The default skip logic is provided by attributes implementing ITestCondition that you write.  If a single attribute on the test method returns non null for SkipReason then the test will be skipped.

```csharp
public interface ITestCondition
{
    string SkipReason { get; }
}
```

For instance 

```csharp
public class TestCondition:Attribute, ITestCondition
{
    public TestCondition(){}
    public TestCondition(string skipReason)
    {
        SkipReason = skipReason;
    }
    public string SkipReason { get; }
}

public class TestClass {

  [FactConditional]
  [TestCondition]
  public void Should_Not_Be_Skipped(){}

  [FactConditional]
  [TestCondition("Because...")]
  public void Should_Be_Skipped(){}
}
```

If the skip logic is not sufficient both FactConditional and TheoryConditional have a Type constructor argument for a type implementing ISkipLogic.  Return non null to skip.

```csharp
public interface ISkipLogic
{
    string GetSkipReason(ITestMethod testMethod);
}
```

For instance

```csharp
public class AlwaysSkip : ISkipLogic{
  string GetSkipReason(ITestMethod _){
    return "Because...";
  }
}

public class TestClass {
  [FactConditional(typeof(AlwaysSkip))]
  public void Should_Be_Skipped(){}
}
```

For custom discoverers you can use the ConditionalFact attribute.

For instance given the following discoverer / attribute pair

```csharp
[XunitTestCaseDiscoverer("CustomDiscoverer.CustomDiscoverer", "CustomDiscoverer")]
public class CustomFactAttribute : FactAttribute {
  public CustomFactAttribute(string someArg){
    //....
  }
}

class CustomDiscoverer : IXUnitTestCaseDiscoverer {
  IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute){
    throw new TBD();
  }
}
```

the following test is conditional.

```csharp
public class TestClass {

  [ConditionalFact(typeof(CustomFactAttribute), new object[]{ "Arg used by the discoverer"})]
  [TestCondition("Because...")]
  public void Should_Be_Skipped(){}
}
```

Again the skip logic can be custom.  Again provide the ISkipLogic type as the final attribute constructor argument.


