using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using WerkstattlagerAPI.Models;
using WerkstattlagerUI.Views;
using WerkstattlagerViewLogic.ViewModels;

namespace WerkstattlagerUI
{
    public partial class MainWindow : Window
    {
        private readonly Inventory _inventory;
        private readonly DeviceOverview _deviceOverview;
        private readonly CategoryOverview _categoryOverview;
        private readonly ManufacturerOverview _manufacturerOverview;

        public MainWindow(Inventory inventory, DeviceOverview deviceOverview, CategoryOverview categoryOverview, ManufacturerOverview manufacturerOverview)
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

            App.Current.DispatcherUnhandledException += (sender, e) =>
            {
                MessageBox.Show(e.Exception.Message, "Unbehandelte Ausnahme", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            };
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            ActivatorUtilities.CreateInstance<CreateItemWindow>(App.RootServiceProvider).ShowDialog();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = string.Empty;
        }

        private void UpdateItem_Click (object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Item item)
                ActivatorUtilities.CreateInstance<UpdateItemWindow>(App.RootServiceProvider, item).ShowDialog();
        }

        private void MoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Item item)
            {
                var moveItemWindow = ActivatorUtilities.CreateInstance<MoveItemWindow>(App.RootServiceProvider);
                if (item.IsInInventory)
                    moveItemWindow.Title = "Item auslagern";
                else
                    moveItemWindow.Title = "Item einlagern";
                moveItemWindow.ShowDialog();
            }
        }

        private void NewRecordButton_Click(object sender, RoutedEventArgs e)
        {
            ActivatorUtilities.CreateInstance<NewRecordSelection>(App.RootServiceProvider).ShowDialog();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _inventory.SearchItems(SearchBox.Text);
        }
    }
}
