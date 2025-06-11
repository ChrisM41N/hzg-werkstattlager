using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WerkstattlagerAPI;
using WerkstattlagerAPI.Models;

namespace WerkstattlagerViewLogic.ViewModels;

public partial class CategoryOverview : ObservableObject
{
    [ObservableProperty] private Category? selectedCategory;
    public ObservableCollection<Category> Categories { get; set; } = [];

    public event Action<string>? Error;

    public CategoryOverview()
    {
        _ = ReadCategories();
    }

    public async Task CreateCategory(Category newCategory)
    {
        try
        {
            using var context = new InventoryContext();
            context.Categories.Add(newCategory);
            await context.SaveChangesAsync();
            Categories.Add(newCategory);
            await ReadCategories();
        }
        catch (DbUpdateException DbException)
        {
            string errorMessage = DbException.InnerException?.Message ?? DbException.Message;
            Debug.WriteLine(errorMessage);
            Error?.Invoke(errorMessage);
        }
    }

    [RelayCommand]
    public async Task ReadCategories()
    {
        using var context = new InventoryContext();
        Categories.Clear();
        var categories = await context.Categories.ToListAsync();
        foreach (var category in categories)
            Categories.Add(category);
    }

    public async Task UpdateCategory(Category updatedCategory)
    {
        try
        {
            using var context = new InventoryContext();
            context.Categories.Update(updatedCategory);
            await context.SaveChangesAsync();
            await ReadCategories();
        }
        catch (DbUpdateException DbException)
        {
            string errorMessage = DbException.InnerException?.Message ?? DbException.Message;
            Debug.WriteLine(errorMessage);
            Error?.Invoke(errorMessage);
        }
    }

    [RelayCommand]
    public async Task DeleteCategory()
    {
        try
        {
            using var context = new InventoryContext();
            if (SelectedCategory != null)
            {
                context.Categories.Remove(SelectedCategory);
                await context.SaveChangesAsync();
                Categories.Remove(SelectedCategory);
                SelectedCategory = null;
                await ReadCategories();
            }
        }
        catch (DbUpdateException DbException)
        {
            string errorMessage = DbException.InnerException?.Message ?? DbException.Message;
            Debug.WriteLine(errorMessage);
            Error?.Invoke(errorMessage);
        }
    }
}