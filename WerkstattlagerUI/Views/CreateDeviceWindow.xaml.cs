using System.Windows;
using WerkstattlagerAPI.Models;
using WerkstattlagerViewLogic.ViewModels;

namespace WerkstattlagerUI.Views
{

    public partial class CreateDeviceWindow : Window
    {
        private readonly DeviceOverview _deviceOverview;
        private readonly CategoryOverview _categoryOverview;
        private readonly ManufacturerOverview _manufacturerOverview;
        public CreateDeviceWindow(DeviceOverview deviceOverview, CategoryOverview categoryOverview, ManufacturerOverview manufacturerOverview)
        {
            InitializeComponent();
            _deviceOverview = deviceOverview;
            _categoryOverview = categoryOverview;
            _manufacturerOverview = manufacturerOverview;

            CategoryField.DataContext = _categoryOverview;
            ManufacturerField.DataContext = _manufacturerOverview;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryField.SelectedItem is Category category && ManufacturerField.SelectedItem is Manufacturer manufacturer)
            {
                var device = new Device
                {
                    Description = DescriptionField.Text,
                    Category = category,
                    Manufacturer = manufacturer,
                };
                _ = _deviceOverview.UpdateDevice(device);
                Close();
            }
        }
    }
}
