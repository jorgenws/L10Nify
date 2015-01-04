using Core;

namespace L10Nify {
    public class MissingLocalizedTextViewModelFactory : IMissingLocalizedTextViewModelFactory {
        public MissingLocalizedTextViewModel Create(MissingLocalizedText missingLocalizedText,
                                                    IQueryModel queryModel) {
            return new MissingLocalizedTextViewModel(missingLocalizedText,
                                                     queryModel);

        }
    }

    public interface IMissingLocalizedTextViewModelFactory {
        MissingLocalizedTextViewModel Create(MissingLocalizedText missingLocalizedText,
                                             IQueryModel queryModel);
    }
}