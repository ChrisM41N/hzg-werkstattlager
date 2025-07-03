using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WerkstattlagerAPI;
using WerkstattlagerViewLogic.ViewModels;

namespace WerkstattlagerIntegration.Tests
{
    public class ManufacturerOverviewTest(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
    {
        private readonly ManufacturerViewModel _manufacturerOverview = new();
        private readonly DatabaseFixture _fixture = fixture;
        private readonly InventoryContext _context = fixture.Context;

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
