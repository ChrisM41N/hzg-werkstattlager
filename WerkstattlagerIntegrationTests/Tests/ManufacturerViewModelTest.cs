using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WerkstattlagerAPI;
using WerkstattlagerViewLogic.ViewModels;
using Xunit.Abstractions;

namespace WerkstattlagerIntegration.Tests
{
    public class ManufacturerViewModelTest : TestBase, IClassFixture<DatabaseFixture>
    {
        private readonly ManufacturerViewModel _manufacturerOverview;
        private readonly DatabaseFixture _fixture;
        private readonly InventoryContext _context;

        public ManufacturerViewModelTest(DatabaseFixture fixture, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _fixture = fixture;
            _context = fixture.Context;
            _manufacturerOverview = RootServiceProvider.GetRequiredService<ManufacturerViewModel>();
        }

        [Fact]
        public async Task TestAddManufacturer()
        {
            await _manufacturerOverview.CreateManufacturer(_fixture.manufacturerToBeAdded);

            var manufacturer = _context.Manufacturers.FirstOrDefault(m => m.Id == 3);

            Assert.Equal(3, manufacturer?.Id);
            Assert.Equal("TestManufacturer3", manufacturer?.Description);
        }

        [Fact]
        public async Task TestUpdateManufacturer()
        {
            _context.Manufacturers.Add(_fixture.manufacturerToBeUpdated);
            await _context.SaveChangesAsync();

            await _manufacturerOverview.UpdateManufacturer(_fixture.manufacturerForUpdating);

            var updatedManufacturer = new InventoryContext().Manufacturers.FirstOrDefault(m => m.Id == _fixture.manufacturerToBeUpdated.Id);

            Assert.Equal("Updated TestManufacturer4", updatedManufacturer?.Description);
        }

        [Fact]
        public async Task TestDeleteManufacturer()
        {
            _context.Manufacturers.Add(_fixture.manufacturerToBeDeleted);
            await _context.SaveChangesAsync();

            _manufacturerOverview.SelectedManufacturer = _fixture.manufacturerToBeDeleted;

            await _manufacturerOverview.DeleteManufacturer();

            var deletedManufacturer = _context.Manufacturers.FirstOrDefault(m => m.Id == _fixture.manufacturerToBeDeleted.Id);

            Assert.Null(deletedManufacturer);
        }
    }
}
