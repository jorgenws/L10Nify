using System;

namespace Core {
    public class LocalizationPersistance {
        public AreaPersitance[] Areas { get; set; }
        public LocalizationKey[] Keys { get; set; }
        public LocalizedText[] Texts { get; set; }
        public Language[] Languages { get; set; }
        public HistoryEntry[] HistoryEntries { get; set; }
    }

    public class AreaPersitance {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string ImageAsBase64String { get; set; }
    }
}