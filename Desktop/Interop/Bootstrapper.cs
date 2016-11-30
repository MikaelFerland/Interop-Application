using Microsoft.Practices.Unity;
using Prism.Unity;
using Interop.Views;
using System.Windows;

using Prism.Modularity;

using Interop.Infrastructure.Interfaces;

using Interop.Modules.Login;
using Interop.Modules.Map;
using Interop.Modules.Targets;
using Interop.Modules.Telemetry;

namespace Interop
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();            
        }

        protected override void ConfigureModuleCatalog()
        {
            ModuleCatalog catalog = (ModuleCatalog)ModuleCatalog;

            catalog.AddModule(typeof(LoginModule));
            catalog.AddModule(typeof(MapModule));
            catalog.AddModule(typeof(TargetsModule));
            catalog.AddModule(typeof(TelemetryModule));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            
            this.RegisterTypeIfMissing(typeof(IHttpService), typeof(Modules.Client.Services.HttpService), true);
            this.RegisterTypeIfMissing(typeof(ITelemetryService), typeof(Modules.Client.Services.TelemetryService), true);
            this.RegisterTypeIfMissing(typeof(ITargetService), typeof(Modules.Client.Services.TargetService), true);
        }
    }
}
