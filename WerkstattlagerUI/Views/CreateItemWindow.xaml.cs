using System.Windows;
using WerkstattlagerAPI.Models;
using WerkstattlagerViewLogic.ViewModels;

namespace WerkstattlagerUI.Views
{
    public partial class CreateItemWindow : Window
    {
        private readonly Inventory _inventory;
        private readonly DeviceOverview _deviceOverview;
        public CreateItemWindow(Inventory inventory, DeviceOverview deviceOverview)
        {
            InitializeComponent();

            _inventory = inventory;
            _deviceOverview = deviceOverview;

            DataContext = _deviceOverview;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DeviceField.SelectedItem is Device device)
            {
                var item = new Item
                {
                    Id = IdField.Text,
                    SerialNumber = SerialNumberField.Text,
                    DeviceId = device.Id,
                    CommentIn = CommentField.Text,
                };

                _ = _inventory.CreateItem(item);
                Close();
            }
        }
    }
}
