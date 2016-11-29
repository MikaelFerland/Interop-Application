using Microsoft.Practices.Unity;
using Prism.Unity;
using Interop.Views;
using System.Windows;

using Prism.Modularity;
using Interop.Modules.Client;
using Interop.Modules.Login;
using Interop.Modules.Obstacles;
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

            catalog.AddModule(typeof(ClientModule));
            catalog.AddModule(typeof(LoginModule));
            catalog.AddModule(typeof(ObstaclesModule));
            catalog.AddModule(typeof(TargetsModule));
            catalog.AddModule(typeof(TelemetryModule));
        }       
    }
}
