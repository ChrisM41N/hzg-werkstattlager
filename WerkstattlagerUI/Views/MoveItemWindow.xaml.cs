using System.Windows;
using WerkstattlagerViewLogic.ViewModels;

namespace WerkstattlagerUI.Views
{
    public partial class MoveItemWindow : Window
    {
        private readonly ItemViewModel _inventory;

        public MoveItemWindow(ItemViewModel inventory)
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
