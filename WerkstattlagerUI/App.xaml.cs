using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows;
using WerkstattlagerAPI;
using WerkstattlagerViewLogic;
using WerkstattlagerViewLogic.ViewModels;

namespace WerkstattlagerUI
{
    public partial class App : Application
    {
        public readonly static IServiceProvider RootServiceProvider;

        static App()
        {
            var services = new ServiceCollection();
            services.AddWerkstattlager();
            //services.AddLogging(builder =>
            //{
            //    builder.SetMinimumLevel(LogLevel.Debug);
            //    builder.AddDebug();
            //});
            services.AddSingleton<MainWindow>();

            RootServiceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            RootServiceProvider.GetRequiredService<MainWindow>().Show();
        }
    }
}
