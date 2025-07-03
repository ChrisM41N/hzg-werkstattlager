using System.Windows;
using System.Windows.Controls;
using WerkstattlagerAPI.Models;
using WerkstattlagerUI.Views;
using WerkstattlagerViewLogic.ViewModels;

namespace WerkstattlagerUI
{
    public partial class MainWindow : Window
    {
        private readonly ItemViewModel _inventory;
        private readonly DeviceViewModel _deviceOverview;
        private readonly CategoryViewModel _categoryOverview;
        private readonly ManufacturerViewModel _manufacturerOverview;

        public MainWindow(ItemViewModel inventory, DeviceViewModel deviceOverview, CategoryViewModel categoryOverview, ManufacturerViewModel manufacturerOverview)
        {
            InitializeComponent();
            _inventory = inventory;
            _deviceOverview = deviceOverview;
            _categoryOverview = categoryOverview;
            _manufacturerOverview = manufacturerOverview;

            MainGrid.DataContext = inventory;
            Devices.DataContext = deviceOverview;
            Categories.DataContext = categoryOverview;
            Manufacturers.DataContext = manufacturerOverview;

            _inventory.Error += message => MessageBox.Show(message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            _deviceOverview.Error += message => MessageBox.Show(message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            _categoryOverview.Error += message => MessageBox.Show(message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            _manufacturerOverview.Error += message => MessageBox.Show(message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            new CreateItemWindow(_inventory, _deviceOverview).ShowDialog();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = string.Empty;
        }

        private void UpdateItem_Click (object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Item item)
                new UpdateItemWindow(_inventory, _deviceOverview, item).ShowDialog();
        }

        private void MoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Item item)
            {
                var moveItemWindow = new MoveItemWindow(_inventory);
                if (item.IsInInventory)
                    moveItemWindow.Title = "Item auslagern";
                else
                    moveItemWindow.Title = "Item einlagern";
                moveItemWindow.ShowDialog();
            }
        }

        private void NewRecordButton_Click(object sender, RoutedEventArgs e)
        {
            new NewRecordSelection(_deviceOverview, _categoryOverview, _manufacturerOverview).ShowDialog();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _inventory.SearchItems(SearchBox.Text);
        }
    }
}
