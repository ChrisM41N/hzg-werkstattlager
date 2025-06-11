using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WerkstattlagerAPI;
using WerkstattlagerAPI.Models;

namespace WerkstattlagerViewLogic.ViewModels
{
    public partial class ManufacturerOverview : ObservableObject
    {
        [ObservableProperty] private Manufacturer? selectedManufacturer;
        public ObservableCollection<Manufacturer> Manufacturers { get; set; } = [];

        public event Action<string>? Error;

        public ManufacturerOverview()
        {
            _ = ReadManufacturers();
        }

        public async Task CreateManufacturer(Manufacturer newManufacturer)
        {
            try
            {
                using var context = new InventoryContext();
                context.Manufacturers.Add(newManufacturer);
                await context.SaveChangesAsync();
                Manufacturers.Add(newManufacturer);
                await ReadManufacturers();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                string errorMessage = ex.InnerException is SqlException ? ex.InnerException.Message : ex.Message;
                Error?.Invoke(errorMessage);
            }
        }

        [RelayCommand]
        public async Task ReadManufacturers()
        {
            using var context = new InventoryContext();
            Manufacturers.Clear();
            var manufacturers = await context.Manufacturers.ToListAsync();
            foreach (var manufacturer in manufacturers)
                Manufacturers.Add(manufacturer);
        }

        public async Task UpdateManufacturer(Manufacturer updatedManufacturer)
        {
            try
            {
                using var context = new InventoryContext();
                context.Manufacturers.Update(updatedManufacturer);
                await context.SaveChangesAsync();
                await ReadManufacturers();
            }
            catch(DbUpdateException DbException)
            {
                string errorMessage = DbException.InnerException is SqlException sqlException ? sqlException.Message : DbException.Message;
                Debug.WriteLine(errorMessage);
                Error?.Invoke(errorMessage);
            }
        }

        [RelayCommand]
        public async Task DeleteManufacturer()
        {
            try
            {
                using var context = new InventoryContext();
                if (SelectedManufacturer != null)
                {
                    context.Manufacturers.Remove(SelectedManufacturer);
                    await context.SaveChangesAsync();
                    Manufacturers.Remove(SelectedManufacturer);
                    SelectedManufacturer = null;
                    await ReadManufacturers();
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
}
