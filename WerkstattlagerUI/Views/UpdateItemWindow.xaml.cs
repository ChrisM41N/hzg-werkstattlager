using System.Windows;
using WerkstattlagerAPI.Models;
using WerkstattlagerViewLogic.ViewModels;

namespace WerkstattlagerUI
{
    public partial class UpdateItemWindow : Window
    {
        private readonly Inventory _inventory;
        private readonly DeviceOverview _deviceOverview;
        private readonly Item _item;

        public UpdateItemWindow(Inventory inventory, DeviceOverview deviceOverview, Item item)
        {
            InitializeComponent();
            _inventory = inventory;
            _deviceOverview = deviceOverview;
            _item = item;

            DataContext = _deviceOverview;

            IdFieldOld.Text = item.Id!;
            IdFieldNew.Text = item.Id!;

            SerialNumberFieldOld.Text = item.SerialNumber;
            SerialNumberFieldNew.Text = item.SerialNumber;

            DeviceFieldOld.Text = item.Device!.Description;
            DeviceFieldNew.SelectedItem = _deviceOverview.Devices.FirstOrDefault(d => d.Id == item.Device.Id);

            if (item.IsInInventory)
            {
                CommentFieldOld.Text = item.CommentIn;
                CommentFieldNew.Text = item.CommentIn;
            }
            else
            {
                CommentFieldOld.Text = item.CommentOut;
                CommentFieldNew.Text = item.CommentOut;
            }

            DeviceFieldNew.SelectedItem = item.Device;

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DeviceFieldNew.SelectedItem is Device device)
            {
                var item = new Item
                {
                    Id = IdFieldNew.Text,
                    SerialNumber = SerialNumberFieldNew.Text,
                    DeviceId = device.Id,
                    CommentIn = CommentFieldNew.Text,
                    IsInInventory = _item.IsInInventory,
                    DateOut = DateTime.UtcNow,
                };

                _ = _inventory.UpdateItem(item);
                Close();
            }
        }
    }
}
