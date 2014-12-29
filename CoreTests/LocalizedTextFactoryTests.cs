using System;
using Core;
using Moq;
using NUnit.Framework;

namespace CoreTests {
    [TestFixture]
    public class LocalizedTextFactoryTests {
        private Mock<IGuidGenerator> _guidGenerator;

        private readonly Guid _id = Guid.Parse("{D6C6C702-BCA7-4A26-8981-6B5955A41C7A}");
        private readonly Guid _keyId = Guid.Parse("{869FF43C-D26A-4E84-85F5-C3407E9AD72E}");
        private readonly Guid _languageId = Guid.Parse("{CD029CE1-EE43-45BD-887A-4352333DFC2A}");

        [SetUp]
        public void SetUp() {
            _guidGenerator = new Mock<IGuidGenerator>();
            _guidGenerator.Setup(c => c.Next())
                          .Returns(_id);
        }

        [Test]
        public void Create_KeyIdIsEmpty_ThrowsNotSupportedException() {
            var factory = CreateDefaultLocalizedTextFactory();

            Assert.Throws<NotSupportedException>(() => factory.Create(Guid.Empty,
                                                                      _languageId,
                                                                      "test"));
        }

        [Test]
        public void Create_LanguageIdIsEmpty_ThrowsNotSupportedException() {
            var factory = CreateDefaultLocalizedTextFactory();

            Assert.Throws<NotSupportedException>(() => factory.Create(_keyId,
                                                                      Guid.Empty,
                                                                      "test"));
        }

        [Test]
        public void Create_TextIsNull_LocalizedTextIsEmpty() {
            var factory = CreateDefaultLocalizedTextFactory();

            var result = factory.Create(_keyId,
                                        _languageId,
                                        null);

            Assert.AreEqual(string.Empty,
                            result.Text);
        }

        [Test]
        public void Create_TextIsSomething_LocalizedTextIsSomething() {
            const string text = "something";
            var factory = CreateDefaultLocalizedTextFactory();

            var result = factory.Create(_keyId,
                                        _languageId,
                                        text);

            Assert.AreEqual(text,
                            result.Text);
            Assert.AreEqual(_id,
                            result.Id);
            Assert.AreEqual(_keyId,
                            result.KeyId);
            Assert.AreEqual(_languageId,
                            result.LanguageId);
        }

        private ILocalizedTextFactory CreateDefaultLocalizedTextFactory() {
            return new LocalizedTextFactory(_guidGenerator.Object);
        }

    }
}
