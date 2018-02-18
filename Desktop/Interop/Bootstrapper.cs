using Microsoft.Practices.Unity;
using Prism.Unity;
using Interop.Views;
using System.Windows;

using Prism.Modularity;

using Interop.Infrastructure.Interfaces;

using Interop.Modules.UserInterface;
using Interop.Modules.Map;
using Interop.Modules.Details;
using Interop.Modules.Telemetry;

namespace Interop
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();           
        }

        protected override void ConfigureModuleCatalog()
        {
            ModuleCatalog catalog = (ModuleCatalog)ModuleCatalog;

            catalog.AddModule(typeof(UserInterfaceModule));
            catalog.AddModule(typeof(MapModule));
            catalog.AddModule(typeof(DetailsModule));
            catalog.AddModule(typeof(TelemetryModule));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            
            this.RegisterTypeIfMissing(typeof(IHttpService), typeof(Infrastructure.Services.HttpService), true);
            this.RegisterTypeIfMissing(typeof(ITelemetryService), typeof(Infrastructure.Services.TelemetryService), true);
            this.RegisterTypeIfMissing(typeof(ITargetService), typeof(Infrastructure.Services.TargetService), true);
        }
    }
}
