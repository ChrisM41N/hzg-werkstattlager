using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using WerkstattlagerAPI;
using WerkstattlagerAPI.Models;

namespace WerkstattlagerViewLogic.ViewModels;

public partial class DeviceViewModel : ViewModelBase
{
    [ObservableProperty] private Device? selectedDevice;
    [ObservableProperty] public ObservableCollection<Device> devices  = [];

    public event Action<string>? Error;

    public DeviceViewModel(IDbContextFactory<InventoryContext> dbContextFactory, ILogger<DeviceViewModel>? logger = null) : base(logger, dbContextFactory)
    {
        _ = ReadDevices();
    }

    public async Task CreateDevice(Device newDevice)
    {
        try
        {
            using var context = await DbContextFactory.CreateDbContextAsync();
            context.Devices.Add(newDevice);
            await context.SaveChangesAsync();
            Devices.Add(newDevice);
        }
        catch(DbUpdateException DbException)
        {
            string errorMessage = DbException.InnerException is SqlException sqlException ? sqlException.Message : DbException.Message;
            Logger?.LogDebug(DbException, "{message}", errorMessage);
            Error?.Invoke(errorMessage);
        }
    }

    [RelayCommand]
    public async Task ReadDevices()
    {
        using var context = await DbContextFactory.CreateDbContextAsync();
        Devices = new(await context.Devices.Include(d => d.Category).Include(d => d.Manufacturer).ToListAsync());
    }

    public async Task UpdateDevice(Device updatedDevice)
    {
        try
        {
            using var context = await DbContextFactory.CreateDbContextAsync();
            context.Devices.Update(updatedDevice);
            await context.SaveChangesAsync();
            await ReadDevices();
        }
        catch (DbUpdateException DbException)
        {
            string errorMessage = DbException.InnerException is SqlException sqlException ? sqlException.Message : DbException.Message;
            Logger?.LogDebug(DbException, "{message}", errorMessage);
            Error?.Invoke(errorMessage);
        }
    }

    [RelayCommand]
    public async Task DeleteDevice()
    {
        if (SelectedDevice == null)
            return;
        try
        {
            using var context = await DbContextFactory.CreateDbContextAsync();
            context.Devices.Remove(SelectedDevice!);
            await context.SaveChangesAsync();
            Devices.Remove(SelectedDevice!);
            SelectedDevice = null;
            await ReadDevices();
        }
        catch(DbUpdateException DbException)
        {
            string errorMessage = DbException.InnerException is SqlException sqlException ? sqlException.Message : DbException.Message;
            Logger?.LogDebug(DbException, "{message}", errorMessage);
            Error?.Invoke(errorMessage);
        }
    }
}
