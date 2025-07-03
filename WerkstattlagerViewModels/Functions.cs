using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WerkstattlagerAPI;
using WerkstattlagerViewLogic.ViewModels;

namespace WerkstattlagerViewLogic;

public static class Functions
{
    public static IServiceCollection AddWerkstattlager(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddDbContextFactory<InventoryContext>()
            .AddSingleton<ItemViewModel>()
            .AddSingleton<DeviceViewModel>()
            .AddSingleton<CategoryViewModel>()
            .AddSingleton<ManufacturerViewModel>()
            ;
    }
}
