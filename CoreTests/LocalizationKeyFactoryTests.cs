using System;
using Core;
using Moq;
using NUnit.Framework;

namespace CoreTests {
    [TestFixture]
    public class LocalizationKeyFactoryTests {
        private Mock<IGuidGenerator> _guidGenerator;

        private readonly Guid _id = Guid.Parse("{FA31C799-CBC5-4D64-B4DE-E2328B70FD3E}");
        private readonly Guid _areaId = Guid.Parse("{DA6D515C-2372-4CDF-9A9D-047D8B9B90A3}");

        [SetUp]
        public void SetUp() {
            _guidGenerator = new Mock<IGuidGenerator>();
            _guidGenerator.Setup(c => c.Next())
                          .Returns(_id);
        }

        [Test]
        public void Create_AreaIdIsEmpty_ThrowsNotSupportedException() {
            var factory = CreateDefaultLocalizationKeyFactory();

            Assert.Throws<NotSupportedException>(() => factory.Create(Guid.Empty,
                                                                      "something"));
        }

        [Test]
        public void Create_KeyIsNull_KeyIsEmpty() {
            var factory = CreateDefaultLocalizationKeyFactory();

            var result = factory.Create(_areaId,
                                        null);

            Assert.AreEqual(_areaId,
                            result.AreaId);
            Assert.AreEqual(string.Empty,
                            result.Key);
            Assert.AreEqual(_id,
                            result.Id);
        }

        [Test]
        public void Create_KeyHasValue_KeyGetsValue() {
            const string key = "key";
            var factory = CreateDefaultLocalizationKeyFactory();

            var result = factory.Create(_areaId,
                                        key);

            Assert.AreEqual(_areaId,
                            result.AreaId);
            Assert.AreEqual(key,
                            result.Key);
            Assert.AreEqual(_id,
                            result.Id);
        }

        private LocalizationKeyFactory CreateDefaultLocalizationKeyFactory() {
            return new LocalizationKeyFactory(_guidGenerator.Object);
        }
    }
}