using System.Windows;
using WerkstattlagerViewLogic.ViewModels;

namespace WerkstattlagerUI.Views
{
    public partial class NewRecordSelection : Window
    {
        private readonly DeviceViewModel _deviceOverview;
        private readonly CategoryViewModel _categoryOverview;
        private readonly ManufacturerViewModel _manufacturerOverview;
        public NewRecordSelection(DeviceViewModel deviceOverview, CategoryViewModel categoryOverview, ManufacturerViewModel manufacturerOverview)
        {
            InitializeComponent();
            _deviceOverview = deviceOverview;
            _categoryOverview = categoryOverview;
            _manufacturerOverview = manufacturerOverview;
        }

        private void NewDeviceButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new CreateDeviceWindow(_deviceOverview, _categoryOverview, _manufacturerOverview).ShowDialog();
        }

        private void NewCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new CreateCategoryWindow(_categoryOverview).ShowDialog();
        }

        private void NewManufacturerButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new CreateManufacturerWindow(_manufacturerOverview).ShowDialog();
        }
    }
}
