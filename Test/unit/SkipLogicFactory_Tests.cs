using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using XUnit_Conditional_Fact;

namespace Test.unit
{
    public class SkipLogicFactory_Tests
    {
        [Fact]
        public void Should_Default_To_TestConditionAttributeIsMetSkipLogic()
        {
            var skipLogicFactory = new SkipLogicFactory();
            var skipLogic = skipLogicFactory.CreateOrDefault(null);
            Assert.Same(typeof(TestConditionAttributeIsMetSkipLogic), skipLogic.GetType());
        }

        [Fact]
        public void Should_Create_Instance_Of_SkipLogicType_If_Provided()
        {
            var skipLogicType = FakeSkipLogic.Type;
            var reflectionHelper = new Mock<IReflectionHelper>();
            var mockedSkipLogic = new Mock<ISkipLogic>().Object;
            reflectionHelper.Setup(rh => rh.CreateInstance<ISkipLogic>(skipLogicType, It.IsAny<object[]>())).Returns(mockedSkipLogic);
            var skipLogicFactory = new SkipLogicFactory(reflectionHelper.Object);
            
            var skipLogic = skipLogicFactory.CreateOrDefault(skipLogicType);
            Assert.Same(mockedSkipLogic, skipLogic);
        }
    }
}
