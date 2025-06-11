using System.Windows;
using WerkstattlagerAPI.Models;
using WerkstattlagerViewLogic.ViewModels;

namespace WerkstattlagerUI.Views
{
    public partial class CreateManufacturerWindow : Window
    {
        private readonly ManufacturerOverview _manufacturerOverview;
        public CreateManufacturerWindow(ManufacturerOverview manufacturerOverview)
        {
            InitializeComponent();
            _manufacturerOverview = manufacturerOverview;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var manufacturer = new Manufacturer()
            {
                Description = DescriptionField.Text
            };
            _ = _manufacturerOverview.CreateManufacturer(manufacturer);
            Close();
        }
    }
}
