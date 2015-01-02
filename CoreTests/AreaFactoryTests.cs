using System;
using Core;
using NUnit.Framework;

namespace CoreTests {
    [TestFixture]
    public class AreaFactoryTests {
        private readonly Guid _id = Guid.Parse("{79806B5F-0CF0-4665-A6BA-7300635D52A2}");

        [Test]
        public void Create_AreaIdIsEmpty_ThrowsNotSupportedException() {
            var factory = CreateDefaultAreaFactory();
            Assert.Throws<NotSupportedException>(() => factory.Create(Guid.Empty,
                                                                      "name",
                                                                      "comment",
                                                                      null));
        }

        [Test]
        public void Create_NameIsNull_NameIsSetToEmptyString() {
            var factory = CreateDefaultAreaFactory();

            var result = factory.Create(_id,
                                        null,
                                        "Comment",
                                        null);

            Assert.AreEqual(_id,
                            result.Id);
            Assert.AreEqual(string.Empty,
                            result.Name);
        }

        [Test]
        public void Create_NameHasValue_NameIsSet() {
            const string name = "name";
            var factory = CreateDefaultAreaFactory();

            var result = factory.Create(_id,
                                        name,
                                        null,
                                        null);

            Assert.AreEqual(_id,
                            result.Id);
            Assert.AreEqual(name,
                            result.Name);
        }

        private AreaFactory CreateDefaultAreaFactory() {
            return new AreaFactory();
        }
    }
}
