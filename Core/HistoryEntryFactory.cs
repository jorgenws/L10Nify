using System;

namespace Core {
    public class HistoryEntryFactory : IHistoryEntryFactory {
        private readonly IDifferenceFinder _differenceFinder;

        public HistoryEntryFactory(IDifferenceFinder differenceFinder) {
            _differenceFinder = differenceFinder;
        }

        public HistoryEntry Create(ILocalization localization,
                                   ILoadedLocalization previousLocalization) {
            //ToDo: Use difference finder to generate text description of what has changed.

            //_differenceFinder.


            return new HistoryEntry(DateTime.Now,
                                    "");
        }
    }

    public interface IHistoryEntryFactory {
        HistoryEntry Create(ILocalization localization,
                            ILoadedLocalization previousLocalization);
    }
}