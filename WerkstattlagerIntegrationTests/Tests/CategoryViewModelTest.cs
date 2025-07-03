using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WerkstattlagerAPI;
using WerkstattlagerViewLogic.ViewModels;
using Xunit.Abstractions;

namespace WerkstattlagerIntegration.Tests
{
    public class CategoryViewModelTest : TestBase, IClassFixture<DatabaseFixture>
    {
        private readonly CategoryViewModel _categoryOverview;
        private readonly DatabaseFixture _fixture;
        private readonly InventoryContext _context;

        public CategoryViewModelTest(DatabaseFixture fixture, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _fixture = fixture;
            _context = fixture.Context;
            _categoryOverview = RootServiceProvider.GetRequiredService<CategoryViewModel>();
        }

        [Fact]
        public async Task TestCreateCategory()
        {
            await _categoryOverview.CreateCategory(_fixture.categoryToBeAdded);

            var category = _context.Categories.FirstOrDefault(c => c.Id == _fixture.categoryToBeAdded.Id);

            Assert.Equal('C', category?.Id);
            Assert.Equal("TestCategory3", category?.Description);
        }

        [Fact]
        public async Task TestUpdateCategory()
        {
            _context.Categories.Add(_fixture.categoryToBeUpdated);
            await _context.SaveChangesAsync();

            await _categoryOverview.UpdateCategory(_fixture.categoryForUpdating);

            var updatedCategory = new InventoryContext().Categories.FirstOrDefault(c => c.Id == _fixture.categoryToBeUpdated.Id);

            Assert.Equal("Updated TestCategory4", updatedCategory?.Description);
        }

        [Fact]
        public async Task TestDeleteCategory()
        {
            _context.Categories.Add(_fixture.categoryToBeDeleted);
            await _context.SaveChangesAsync();

            _categoryOverview.SelectedCategory = _fixture.categoryToBeDeleted;

            await _categoryOverview.DeleteCategory();

            var deletedCategory = _context.Categories.FirstOrDefault(c => c.Id == _fixture.categoryToBeDeleted.Id);

            Assert.Null(deletedCategory);
        }
    }
}
