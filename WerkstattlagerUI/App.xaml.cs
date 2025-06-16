using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WerkstattlagerViewLogic.ViewModels;

namespace WerkstattlagerUI
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            services.AddSingleton<Inventory>();
            services.AddSingleton<DeviceOverview>();
            services.AddSingleton<CategoryOverview>();
            services.AddSingleton<ManufacturerOverview>();
            services.AddSingleton<MainWindow>();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _serviceProvider.GetRequiredService<MainWindow>().Show();
        }
    }
}
