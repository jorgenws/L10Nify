using System;

namespace Core {
    public class Language {
        public Guid Id { get; set; }
        public string LanguageRegion { get; set; }
        public int LCID { get; set; }
        public string DisplayName { get; set; }
    }
}