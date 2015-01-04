using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using Core;
using Ninject;

namespace L10Nify {
    public class AppBootstrapper : BootstrapperBase {
        private StandardKernel _kernel;

        public AppBootstrapper() {
            Initialize();
        }

        protected override void Configure() {
            _kernel = new StandardKernel();

            _kernel.Bind<IWindowManager>()
                   .To<WindowManager>()
                   .InSingletonScope();
            _kernel.Bind<IEventAggregator>()
                   .To<EventAggregator>()
                   .InSingletonScope();
            _kernel.Bind<IShell>()
                   .To<ShellViewModel>();

            //Core
            _kernel.Bind<IAreaFactory>()
                   .To<AreaFactory>()
                   .InSingletonScope();
            _kernel.Bind<ILocalizationKeyFactory>()
                   .To<LocalizationKeyFactory>()
                   .InSingletonScope();
            _kernel.Bind<ILocalizedTextFactory>()
                   .To<LocalizedTextFactory>()
                   .InSingletonScope();
            _kernel.Bind<ILanguageFactory>()
                   .To<LanguageFactory>()
                   .InSingletonScope();
            _kernel.Bind<IHistoryEntryFactory>()
                   .To<HistoryEntryFactory>()
                   .InSingletonScope();
            _kernel.Bind<ILocalizationBuilder>()
                   .To<LocalizationBuilder>()
                   .InSingletonScope();
            _kernel.Bind<IDifferenceFinder>()
                   .To<DifferenceFinder>()
                   .InSingletonScope();
            _kernel.Bind<Model>()
                   .To<Model>()
                   .InSingletonScope();
            _kernel.Bind<IQueryModel>()
                   .ToMethod((ctx) => ctx.Kernel.Get<Model>());
            _kernel.Bind<ILocalizationPersister>()
                   .To<JsonLocalizationPersister>()
                   .InSingletonScope();
            _kernel.Bind<ILocalizationLoader>()
                   .To<JsonLocalizationLoader>()
                   .InSingletonScope();
            _kernel.Bind<IGuidGenerator>()
                   .To<GuidGenerator>()
                   .InSingletonScope();

            //Command infrastructure
            _kernel.Bind<ICommandInvoker>()
                   .To<CommandInvoker>()
                   .InSingletonScope();
            _kernel.Bind<ICommandHandler>()
                   .To<LocalizationHandler>()
                   .InSingletonScope();

            //ViewModels
            _kernel.Bind<IAreaViewModelFactory>()
                   .To<AreaViewModelFactory>()
                   .InSingletonScope();
            _kernel.Bind<ITreeViewModelBuilder>()
                   .To<TreeViewModelBuilder>()
                   .InSingletonScope();
            _kernel.Bind<IWorkbenchFactory>()
                   .To<WorkbenchFactory>()
                   .InSingletonScope();
            _kernel.Bind<IMissingLocalizedTextViewModelFactory>()
                   .To<MissingLocalizedTextViewModelFactory>()
                   .InSingletonScope();
            _kernel.Bind<IMissingLocalizedTextsViewModel>()
                   .To<MissingLocalizedTextsViewModel>()
                   .InSingletonScope();
        }

        protected override object GetInstance(Type service,
                                              string key) {
            if (service != null)
                return _kernel.Get(service);

            throw new ArgumentNullException("service");
        }

        protected override IEnumerable<object> GetAllInstances(Type service) {
            return _kernel.GetAll(service);
        }

        protected override void BuildUp(object instance) {
            _kernel.Inject(instance);
        }

        protected override IEnumerable<Assembly> SelectAssemblies() {
            return new[] {
                             Assembly.GetExecutingAssembly(),
                             typeof (Model).Assembly
                         };
        }

        protected override void OnStartup(object sender,
                                          StartupEventArgs e) {
            DisplayRootViewFor<IShell>();
        }
    }
}