namespace Core {
    public class LocalizationPersistance {
        public Area[] Areas { get; set; }
        public LocalizationKey[] Keys { get; set; }
        public LocalizedText[] Texts { get; set; }
        public Language[] Languages { get; set; }
        public HistoryEntry[] HistoryEntries { get; set; }
    }
}