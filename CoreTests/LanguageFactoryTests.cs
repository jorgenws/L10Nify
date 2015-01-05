using System;
using Core;
using NUnit.Framework;

namespace CoreTests {
    [TestFixture]
    public class LanguageFactoryTests {
        private readonly Guid _id = Guid.Parse("{26766087-2DB4-469B-8BA0-5F265C5DFF01}");
        private const string IsoName = "NO";
        private const string DisplayName = "Norsk";
        private const int LCID = 1;

        [Test]
        public void Create_LanguageIdIsEmpty_ThrowsNotSupportedException() {
            var factory = CreateLanguageFactory();

            Assert.Throws<NotSupportedException>(() => factory.Create(Guid.Empty,
                                                                      IsoName,
                                                                      LCID,
                                                                      DisplayName));
        }

        [Test]
        public void Create_HasValidArguments_CreatesLanguage() {
            var factory = CreateLanguageFactory();

            var language = factory.Create(_id,
                                          IsoName,
                                          LCID,
                                          DisplayName);
            Assert.AreEqual(_id,
                            language.Id);
            Assert.AreEqual(IsoName,
                            language.LanguageRegion);
            Assert.AreEqual(DisplayName,
                            language.DisplayName);
            Assert.AreEqual(LCID,
                            language.LCID);

        }

        private LanguageFactory CreateLanguageFactory() {
            return new LanguageFactory();
        }
    }
}