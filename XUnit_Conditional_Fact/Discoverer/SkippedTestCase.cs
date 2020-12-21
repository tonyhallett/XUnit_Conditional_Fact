using System;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace XUnit_Conditional_Fact
{
    internal class SkippedTestCase : XunitTestCase
    {
        private string _skipReason;

        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public SkippedTestCase() { }

        public SkippedTestCase(string skipReason, IMessageSink diagnosticMessageSink, TestMethodDisplay defaultMethodDisplay, TestMethodDisplayOptions defaultMethodDisplayOptions, ITestMethod testMethod)
            : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod, null)
        {
            _skipReason = skipReason;
        }
        protected override string GetSkipReason(IAttributeInfo factAttribute)
        {
            return _skipReason;
        }

        public override void Serialize(IXunitSerializationInfo data)
        {
            base.Serialize(data);
            data.AddValue(nameof(_skipReason), _skipReason, null);
        }
        public override void Deserialize(IXunitSerializationInfo data)
        {
            this._skipReason = data.GetValue<string>(nameof(_skipReason));
            base.Deserialize(data);
        }
    }
    
}
