using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WerkstattlagerAPI;
using WerkstattlagerAPI.Models;

namespace WerkstattlagerIntegration;

public class DatabaseFixture : IDisposable
{
    public Category category1 = new() { Id = 'A', Description = "TestCategory1" };
    public Category category2 = new() { Id = 'B', Description = "TestCategory2" };
    public Category categoryToBeAdded = new() { Id = 'C', Description = "TestCategory3" };
    public Category categoryToBeUpdated = new() { Id = 'D', Description = "TestCategory4" };
    public Category categoryForUpdating = new() { Id = 'D', Description = "Updated TestCategory4" };
    public Category categoryToBeDeleted = new() { Id = 'E', Description = "TestCategory5" };

    public Manufacturer manufacturer = new() { Id = 1, Description = "TestManufacturer1" };
    public Manufacturer manufacturer2 = new() { Id = 2, Description = "TestManufacturer2" };
    public Manufacturer manufacturerToBeAdded = new() { Id = 3, Description = "TestManufacturer3" };
    public Manufacturer manufacturerToBeUpdated = new() { Id = 4, Description = "TestManufacturer4" };
    public Manufacturer manufacturerForUpdating = new() { Id = 4, Description = "Updated TestManufacturer4" };
    public Manufacturer manufacturerToBeDeleted = new() { Id = 5, Description = "TestManufacturer5" };

    public Device device1 = new() { Id = 1, Description = "TestDevice1", CategoryId = 'A', ManufacturerId = 1 };
    public Device device2 = new() { Id = 2, Description = "TestDevice2", CategoryId = 'B', ManufacturerId = 2 };
    public Device deviceToBeAdded = new() { Id = 3, Description = "TestDevice3", CategoryId = 'A', ManufacturerId = 1 };
    public Device deviceToBeUpdated = new() { Id = 4, Description = "TestDevice4", CategoryId = 'A', ManufacturerId = 1 };
    public Device deviceForUpdating = new() { Id = 4, Description = "Updated TestDevice4", CategoryId = 'B', ManufacturerId = 2 };
    public Device deviceToBeDeleted = new() { Id = 5, Description = "TestDevice5", CategoryId = 'A', ManufacturerId = 1 };

    public Item itemToBeAdded = new() { Id = "000001", SerialNumber = "SN000001", DeviceId = 1, CommentIn = "Testing creation" };
    public Item itemToBeUpdated = new() { Id = "000002", SerialNumber = "SN000002", DeviceId = 1, CommentIn = "Testing updating" };
    public Item itemForUpdating = new() { Id = "000002", SerialNumber = "SN000021", DeviceId = 2, CommentIn = "updated" };
    public Item itemToBeDeleted = new() { Id = "000003", SerialNumber = "SN000003", DeviceId = 1, CommentIn = "Delete" };
    public Item itemToBeMoved = new() { Id = "000004", SerialNumber = "SN000004", DeviceId = 1, CommentIn = "Move" };

    private readonly InventoryContext _context;
    public InventoryContext Context => _context;

    public DatabaseFixture()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        _context = new InventoryContext();
        _context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
