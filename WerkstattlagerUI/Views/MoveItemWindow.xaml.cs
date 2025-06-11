using System.Windows;
using WerkstattlagerViewLogic.ViewModels;

namespace WerkstattlagerUI.Views
{
    public partial class MoveItemWindow : Window
    {
        private readonly Inventory _inventory;

        public MoveItemWindow(Inventory inventory)
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
