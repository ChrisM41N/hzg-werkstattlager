using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.ServiceProcess;
using System.Windows;
using WerkstattlagerAPI;
using WerkstattlagerUI.Views;
using WerkstattlagerViewLogic.ViewModels;

namespace WerkstattlagerUI
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            services.AddDbContext<InventoryContext>();
            services.AddSingleton<Inventory>();
            services.AddSingleton<DeviceOverview>();
            services.AddSingleton<CategoryOverview>();
            services.AddSingleton<ManufacturerOverview>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<CreateItemWindow>();
            services.AddSingleton<UpdateItemWindow>();
            services.AddSingleton<MoveItemWindow>();
            services.AddSingleton<NewRecordSelection>();
            services.AddSingleton<CreateDeviceWindow>();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _serviceProvider.GetRequiredService<MainWindow>().Show();
        }
    }
}
