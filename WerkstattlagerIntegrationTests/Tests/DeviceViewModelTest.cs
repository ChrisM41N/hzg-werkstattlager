﻿using Microsoft.Extensions.DependencyInjection;
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
    public class DeviceViewModelTest : TestBase, IClassFixture<DatabaseFixture>
    {
        private readonly DeviceViewModel _deviceOverview;
        private readonly DatabaseFixture _fixture;
        private readonly InventoryContext _context;

        public DeviceViewModelTest(DatabaseFixture fixture, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _fixture = fixture;
            _context = fixture.Context;
            _deviceOverview = RootServiceProvider.GetRequiredService<DeviceViewModel>();
        }

        [Fact]
        public async Task TestCreateDevice()
        {
            await _deviceOverview.CreateDevice(_fixture.deviceToBeAdded);

            var device = _context.Devices.FirstOrDefault(d => d.Id == _fixture.deviceToBeAdded.Id);

            Assert.Equal(3, device?.Id);
            Assert.Equal("TestDevice3", device?.Description);
            Assert.Equal('A', device?.CategoryId);
            Assert.Equal(1, device?.ManufacturerId);
        }

        [Fact]
        public async Task TestUpdateDevice()
        {
            _context.Devices.Add(_fixture.deviceToBeUpdated);
            await _context.SaveChangesAsync();

            await _deviceOverview.UpdateDevice(_fixture.deviceForUpdating);

            var updatedDevice = new InventoryContext().Devices.FirstOrDefault(d => d.Id == _fixture.deviceToBeUpdated.Id);

            Assert.Equal("Updated TestDevice4", updatedDevice?.Description);
            Assert.Equal('B', updatedDevice?.CategoryId);
            Assert.Equal(2, updatedDevice?.ManufacturerId);
        }

        [Fact]
        public async Task TestDeleteDevice()
        {
            _context.Devices.Add(_fixture.deviceToBeDeleted);
            await _context.SaveChangesAsync();

            _deviceOverview.SelectedDevice = _fixture.deviceToBeDeleted;

            await _deviceOverview.DeleteDevice();
            var deletedDevice = _context.Devices.FirstOrDefault(d => d.Id == _fixture.deviceToBeDeleted.Id);

            Assert.Null(deletedDevice);
        }
    }
}
