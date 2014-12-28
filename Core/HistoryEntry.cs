using System;

namespace Core {
    public class HistoryEntry {
        public DateTime SaveDate { get; private set; }
        public string Changes { get; private set; }

        public HistoryEntry(DateTime saveDate,
                            string changes) {
            SaveDate = saveDate;
            Changes = changes;
        }
    }
}