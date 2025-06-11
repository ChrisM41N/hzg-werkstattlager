using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WerkstattlagerAPI;
using WerkstattlagerAPI.Models;

namespace WerkstattlagerViewLogic.ViewModels;

public partial class DeviceOverview : ObservableObject
{
    [ObservableProperty] private Device? selectedDevice;
    public ObservableCollection<Device> Devices { get; set; } = [];

    public event Action<string>? Error;

    public DeviceOverview()
    {
        _ = ReadDevices();
    }

    public async Task CreateDevice(Device newDevice)
    {
        try
        {
            using var context = new InventoryContext();
            context.Devices.Add(newDevice);
            await context.SaveChangesAsync();
            Devices.Add(newDevice);
            await ReadDevices();
        }
        catch(DbUpdateException DbException)
        {
            string errorMessage = DbException.InnerException is SqlException sqlException ? sqlException.Message : DbException.Message;
            Debug.WriteLine(errorMessage);
            Error?.Invoke(errorMessage);
        }
    }

    [RelayCommand]
    public async Task ReadDevices()
    {
        try
        {
            using var context = new InventoryContext();
            Devices.Clear();
            var devices = await context.Devices.Include(d => d.Category).Include(d => d.Manufacturer).ToListAsync();
            foreach (var device in devices)
                Devices.Add(device);
        }
        catch(DbUpdateException DbException)
        {
            string errorMessage = DbException.InnerException is SqlException sqlException ? sqlException.Message : DbException.Message;
            Debug.WriteLine(errorMessage);
            Error?.Invoke(errorMessage);
        }
    }

    public async Task UpdateDevice(Device updatedDevice)
    {
        using var context = new InventoryContext();
        context.Devices.Update(updatedDevice);
        await context.SaveChangesAsync();
        await ReadDevices();
    }

    [RelayCommand]
    public async Task DeleteDevice()
    {
        try
        {
            using var context = new InventoryContext();
            if (SelectedDevice != null)
            {
                context.Devices.Remove(SelectedDevice);
                await context.SaveChangesAsync();
                Devices.Remove(SelectedDevice);
                SelectedDevice = null;
                await ReadDevices();
            }
        }
        catch(DbUpdateException DbException)
        {
            string errorMessage = DbException.InnerException is SqlException sqlException ? sqlException.Message : DbException.Message;
            Debug.WriteLine(errorMessage);
            Error?.Invoke(errorMessage);
        }
    }
}
