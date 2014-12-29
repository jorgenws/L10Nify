using System;
using Core;
using Moq;
using NUnit.Framework;

namespace CoreTests {
    [TestFixture]
    public class AreaFactoryTests {
        private Mock<IGuidGenerator> _guidGenerator;

        private readonly Guid _id = Guid.Parse("{79806B5F-0CF0-4665-A6BA-7300635D52A2}");
        
        [SetUp]
        public void SetUp() {
            _guidGenerator = new Mock<IGuidGenerator>();
            _guidGenerator.Setup(c => c.Next())
                          .Returns(_id);
        }

        [Test]
        public void Create_NameIsNull_NameIsSetToEmptyString() {
            var factory = CreateDefaultAreaFactory();

            var result = factory.Create(null);

            Assert.AreEqual(_id,
                            result.Id);
            Assert.AreEqual(string.Empty,
                            result.Name);
        }

        [Test]
        public void Create_NameHasValue_NameIsSet() {
            const string name = "name";
            var factory = CreateDefaultAreaFactory();

            var result = factory.Create(name);

            Assert.AreEqual(_id,
                            result.Id);
            Assert.AreEqual(name,
                            result.Name);
        }

        private AreaFactory CreateDefaultAreaFactory() {
            return new AreaFactory(_guidGenerator.Object);
        }
    }
}
