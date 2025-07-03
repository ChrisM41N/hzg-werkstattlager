using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using WerkstattlagerAPI;
using WerkstattlagerAPI.Models;

namespace WerkstattlagerViewLogic.ViewModels
{
    public partial class ManufacturerOverview : ViewModelBase
    {
        [ObservableProperty] private Manufacturer? selectedManufacturer;
        [ObservableProperty] private ObservableCollection<Manufacturer> manufacturers = [];

        public event Action<string>? Error;

        public ManufacturerOverview(IDbContextFactory<InventoryContext> dbContextFactory, ILogger<ManufacturerOverview>? logger = null) : base(logger, dbContextFactory)
        {
            _ = ReadManufacturers();
        }

        public async Task CreateManufacturer(Manufacturer newManufacturer)
        {
            try
            {
                using var context = await DbContextFactory.CreateDbContextAsync();
                context.Manufacturers.Add(newManufacturer);
                await context.SaveChangesAsync();
                Manufacturers.Add(newManufacturer);
            }
            catch (Exception DbException)
            {
                string errorMessage = DbException.InnerException is SqlException sqlException ? sqlException.Message : DbException.Message;
                Logger?.LogDebug(DbException, "{message}", errorMessage);
                Error?.Invoke(errorMessage);
            }
        }

        [RelayCommand]
        public async Task ReadManufacturers()
        {
            using var context = await DbContextFactory.CreateDbContextAsync();
            Manufacturers = new(await context.Manufacturers.ToListAsync());
        }

        public async Task UpdateManufacturer(Manufacturer updatedManufacturer)
        {
            try
            {
                using var context = await DbContextFactory.CreateDbContextAsync();
                context.Manufacturers.Update(updatedManufacturer);
                await context.SaveChangesAsync();
                await ReadManufacturers();
            }
            catch(DbUpdateException DbException)
            {
                string errorMessage = DbException.InnerException is SqlException sqlException ? sqlException.Message : DbException.Message;
                Logger?.LogDebug(DbException, "{message}", errorMessage);
                Error?.Invoke(errorMessage);
            }
        }

        [RelayCommand]
        public async Task DeleteManufacturer()
        {
            if (SelectedManufacturer == null)
                return;
            try
            {
                using var context = await DbContextFactory.CreateDbContextAsync();
                context.Manufacturers.Remove(SelectedManufacturer!);
                await context.SaveChangesAsync();
                Manufacturers.Remove(SelectedManufacturer!);
                SelectedManufacturer = null;
                await ReadManufacturers();
            }
            catch (DbUpdateException DbException)
            {
                string errorMessage = DbException.InnerException is SqlException sqlException ? sqlException.Message : DbException.Message;
                Logger?.LogDebug(DbException, "{message}", errorMessage);
                Error?.Invoke(errorMessage);
            }
        }
    }
}
