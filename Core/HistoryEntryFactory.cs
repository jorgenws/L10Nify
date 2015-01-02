using System;

namespace Core {
    public class HistoryEntryFactory : IHistoryEntryFactory {
        private readonly IDifferenceFinder _differenceFinder;

        public HistoryEntryFactory(IDifferenceFinder differenceFinder) {
            _differenceFinder = differenceFinder;
        }

        public HistoryEntry Create(ILocalization localization,
                                   ILoadedLocalization previousLocalization) {
            var changes = _differenceFinder.Changes(previousLocalization,
                                                    localization);

            return new HistoryEntry(DateTime.Now,
                                    changes);
        }
    }

    public interface IHistoryEntryFactory {
        HistoryEntry Create(ILocalization localization,
                            ILoadedLocalization previousLocalization);
    }
}