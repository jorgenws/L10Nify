using System;
using Core;
using NUnit.Framework;

namespace CoreTests {
    [TestFixture]
    public class LanguageFactoryTests {
        private readonly Guid _id = Guid.Parse("{26766087-2DB4-469B-8BA0-5F265C5DFF01}");
        private const string IsoName = "NO";
        private const string DisplayName = "Norsk";

        [Test]
        public void Create_IdIsEmpty_ThrowsNotSupportedException() {
            var factory = new LanguageFactory();
            Assert.Throws<NotSupportedException>
                (() => factory.Create
                           (Guid.Empty,
                            IsoName,
                            DisplayName));
        }

        [Test]
        public void Create_HasValidArguments_CreatesLanguage() {
            var factory = new LanguageFactory();

            var language = factory.Create(_id,
                                          IsoName,
                                          DisplayName);
            Assert.AreEqual(_id, language.Id);
            Assert.AreEqual(IsoName, language.IsoName);
            Assert.AreEqual(DisplayName, language.DisplayName);
        }
    }
}