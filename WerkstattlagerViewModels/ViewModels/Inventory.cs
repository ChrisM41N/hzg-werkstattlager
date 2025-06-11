using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WerkstattlagerAPI;
using WerkstattlagerAPI.Models;

namespace WerkstattlagerViewLogic.ViewModels;

public partial class Inventory : ObservableObject
{
    [ObservableProperty] private Item? selectedItem;
    [ObservableProperty] private string? searchText;
    [ObservableProperty] private string? itemCount;

    public ObservableCollection<Item> AllItemsIn = [];
    public ObservableCollection<Item> AllItemsOut = [];
    [ObservableProperty] public ObservableCollection<Item> itemsIn = [];
    [ObservableProperty] public ObservableCollection<Item> itemsOut = [];

    public event Action<string>? Error;

    public Inventory()
    {
        _ = ReadItems();
    }

    public async Task CreateItem(Item newItem)
    {
        try
        {
            using var context = new InventoryContext();
            context.Items.Add(newItem);
            await context.SaveChangesAsync();
            ItemsIn.Add(newItem);
            await ReadItems();
        }
        catch(DbUpdateException DbException)
        {
            string errorMessage = DbException.InnerException is SqlException sqlException ? sqlException.Message : DbException.Message;
            Debug.WriteLine(errorMessage);
            Error?.Invoke(errorMessage);
        }
    }

    [RelayCommand]
    public async Task ReadItems()
    {
        using var context = new InventoryContext();

        ItemsIn.Clear();
        ItemsOut.Clear();

        var itemsIn = await context.Items.Include(i => i.Device).Include(i => i.Device!.Category).Include(i => i.Device!.Manufacturer).Where(i => i.IsInInventory == true).ToListAsync();
        foreach (var item in itemsIn)
            ItemsIn.Add(item);

        var itemsOut = await context.Items.Include(i => i.Device).Include(i => i.Device!.Category).Include(i => i.Device!.Manufacturer).Where(i => i.IsInInventory == false).ToListAsync();
        foreach (var item in itemsOut)
            ItemsOut.Add(item);

        ItemCount = $"{ItemsIn.Count} Geräte im Lager";

        AllItemsIn = ItemsIn;
        AllItemsOut = ItemsOut;
    }

    public async Task UpdateItem(Item updatedItem)
    {
        try
        {
            using var context = new InventoryContext();
            context.Update(updatedItem);
            await context.SaveChangesAsync();
            await ReadItems();
        }
        catch(DbUpdateException DbException)
        {
            string errorMessage = DbException.InnerException is SqlException sqlException ? sqlException.Message : DbException.Message;
            Debug.WriteLine(errorMessage);
            Error?.Invoke(errorMessage);
        }
    }

    [RelayCommand]
    public async Task DeleteItem()
    {
        try
        {
            using var context = new InventoryContext();
            if (SelectedItem != null)
            {
                context.Items.Remove(SelectedItem);
                await context.SaveChangesAsync();
                if (SelectedItem.IsInInventory)
                    ItemsIn.Remove(SelectedItem);
                else
                    ItemsOut.Remove(SelectedItem);
                SelectedItem = null;
                await ReadItems();
            }
        }
        catch(DbUpdateException DbException)
        {
            string errorMessage = DbException.InnerException is SqlException sqlException ? sqlException.Message : DbException.Message;
            Debug.WriteLine(errorMessage);
            Error?.Invoke(errorMessage);
        }
    }

    public async Task MoveItem(string comment)
    {
        if (SelectedItem != null)
        {
            if (SelectedItem.IsInInventory == true)
            {
                SelectedItem.IsInInventory = false;
                SelectedItem.DateOut = DateTime.UtcNow;
                SelectedItem.CommentOut = comment;
            }
            else
            {
                SelectedItem.IsInInventory = true;
                SelectedItem.DateIn = DateTime.UtcNow;
                SelectedItem.CommentIn = comment;
            }
            await UpdateItem(SelectedItem);
        }
    }

    public void SearchItems(string search)
    {
         var filteredItemsIn = AllItemsIn.Where(item =>
         item.Id!.Contains(
            search, StringComparison.OrdinalIgnoreCase) ||
        (item.SerialNumber!.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
        item.Device!.Description!.Contains(search, StringComparison.OrdinalIgnoreCase) ||
        item.Device.Category!.Description!.Contains(search, StringComparison.OrdinalIgnoreCase) ||
        item.Device.Manufacturer!.Description!.Contains(search, StringComparison.OrdinalIgnoreCase) ||
        (item.CommentIn != null && item.CommentIn.Contains(search, StringComparison.OrdinalIgnoreCase)))
        .ToList();

        ItemsIn = new ObservableCollection<Item>(filteredItemsIn);

        var filteredItemsOut = AllItemsOut.Where(item =>
        (item.Id!.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
        (item.SerialNumber!.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
        (item.Device!.Description!.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
        item.Device.Category!.Description!.Contains(search, StringComparison.OrdinalIgnoreCase) ||
        item.Device.Manufacturer!.Description!.Contains(search, StringComparison.OrdinalIgnoreCase) ||
        (item.CommentOut != null && item.CommentOut.Contains(search, StringComparison.OrdinalIgnoreCase)))
        .ToList();

        ItemsOut = new ObservableCollection<Item>(filteredItemsOut);
    }
}
