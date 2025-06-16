using WerkstattlagerAPI;
using WerkstattlagerViewLogic.ViewModels;

namespace WerkstattlagerIntegration.Tests
{
    public class InventoryTest(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
    {
        private readonly Inventory _inventory = new();
        private readonly DatabaseFixture _fixture = fixture;
        private readonly InventoryContext _context = fixture.Context;

        [Fact]
        public async Task TestCreateItem()
        {
            await _inventory.CreateItem(_fixture.itemToBeAdded);

            var item = _context.Items.FirstOrDefault(i => i.Id == _fixture.itemToBeAdded.Id);

            Assert.Equal("000001", item?.Id);
            Assert.Equal("SN000001", item?.SerialNumber);
            Assert.Equal(1, item?.DeviceId);
            Assert.Equal(_fixture.itemToBeAdded.DateIn, item?.DateIn);
            Assert.Equal("Testing creation", item?.CommentIn);
            Assert.True(item?.IsInInventory);
        }

        [Fact]
        public async Task TestUpdateItem()
        {
            _context.Items.Add(_fixture.itemToBeUpdated);
            await _context.SaveChangesAsync();

            await _inventory.UpdateItem(_fixture.itemForUpdating);

            var updatedItem = new InventoryContext().Items.FirstOrDefault(i => i.Id == _fixture.itemToBeUpdated.Id);

            Assert.Equal("SN000021", updatedItem?.SerialNumber);
            Assert.Equal(2, updatedItem?.DeviceId);
            Assert.Equal("updated", updatedItem?.CommentIn);
        }

        [Fact]
        public async Task TestDeleteItem()
        {
            _context.Items.Add(_fixture.itemToBeDeleted);
            await _context.SaveChangesAsync();

            _inventory.SelectedItem = _fixture.itemToBeDeleted;

            await _inventory.DeleteItem();
            var deletedItem = _context.Items.FirstOrDefault(i => i.Id == _fixture.itemToBeDeleted.Id);

            Assert.Null(deletedItem);
        }

        [Fact]
        public async Task TestMoveItem()
        {
            _context.Items.Add(_fixture.itemToBeMoved);
            await _context.SaveChangesAsync();

            _inventory.SelectedItem = _fixture.itemToBeMoved;

            await _inventory.MoveItem("Moved Out");
            var movedItem = _context.Items.FirstOrDefault(i => i.Id == _fixture.itemToBeMoved.Id);

            Assert.Equal("Moved Out", movedItem?.CommentOut);
            Assert.Equal(_fixture.itemToBeMoved.DateOut, movedItem?.DateOut);
            Assert.False(movedItem?.IsInInventory);
        }
    }
}
