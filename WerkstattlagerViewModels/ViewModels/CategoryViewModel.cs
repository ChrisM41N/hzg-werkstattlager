using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using WerkstattlagerAPI;
using WerkstattlagerAPI.Models;

namespace WerkstattlagerViewLogic.ViewModels;

public partial class CategoryViewModel : ViewModelBase
{
    [ObservableProperty] private Category? selectedCategory;
    [ObservableProperty] private ObservableCollection<Category> categories = [];

    public event Action<string>? Error;

    public CategoryViewModel(IDbContextFactory<InventoryContext> dbContextFactory, ILogger<CategoryViewModel>? logger = null) : base(logger, dbContextFactory)
    {
        _ = ReadCategories();
    }

    public async Task CreateCategory(Category newCategory)
    {
        try
        {
            using var context = await DbContextFactory.CreateDbContextAsync();
            context.Categories.Add(newCategory);
            await context.SaveChangesAsync();
            Categories.Add(newCategory);
        }
        catch (DbUpdateException DbException)
        {
            string errorMessage = DbException.InnerException is SqlException sqlException ? sqlException.Message : DbException.Message;
            Logger?.LogDebug(DbException, "{message}", errorMessage);
            Error?.Invoke(errorMessage);
        }
    }

    [RelayCommand]
    public async Task ReadCategories()
    {
        using var context = await DbContextFactory.CreateDbContextAsync();
        Categories = new(await context.Categories.ToListAsync());
    }

    public async Task UpdateCategory(Category updatedCategory)
    {
        try
        {
            using var context = await DbContextFactory.CreateDbContextAsync();
            context.Categories.Update(updatedCategory);
            await context.SaveChangesAsync();
            await ReadCategories();
        }
        catch (DbUpdateException DbException)
        {
            string errorMessage = DbException.InnerException is SqlException sqlException ? sqlException.Message : DbException.Message;
            Logger?.LogDebug(DbException, "{message}", errorMessage);
            Error?.Invoke(errorMessage);
        }
    }

    [RelayCommand]
    public async Task DeleteCategory()
    {
        if (SelectedCategory == null)
            return;
        try
        {
            using var context = await DbContextFactory.CreateDbContextAsync();
            context.Categories.Remove(SelectedCategory);
            await context.SaveChangesAsync();
            Categories.Remove(SelectedCategory);
            SelectedCategory = null;
            await ReadCategories();
        }
        catch (DbUpdateException DbException)
        {
            string errorMessage = DbException.InnerException is SqlException sqlException ? sqlException.Message : DbException.Message;
            Logger?.LogDebug(DbException, "{message}", errorMessage);
            Error?.Invoke(errorMessage);
        }
    }
}