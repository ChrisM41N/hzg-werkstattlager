using System.Windows;
using WerkstattlagerAPI.Models;
using WerkstattlagerViewLogic.ViewModels;

namespace WerkstattlagerUI.Views
{
    public partial class CreateManufacturerWindow : Window
    {
        private readonly ManufacturerViewModel _manufacturerOverview;
        public CreateManufacturerWindow(ManufacturerViewModel manufacturerOverview)
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
