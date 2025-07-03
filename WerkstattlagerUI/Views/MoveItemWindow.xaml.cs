using System.Windows;
using WerkstattlagerViewLogic.ViewModels;

namespace WerkstattlagerUI.Views
{
    public partial class MoveItemWindow : Window
    {
        private readonly InventoryViewModel _inventory;

        public MoveItemWindow(InventoryViewModel inventory)
        {
            InitializeComponent();
            _inventory = inventory;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _ = _inventory.MoveItem(CommentField.Text);
            Close();
        }
    }
}
