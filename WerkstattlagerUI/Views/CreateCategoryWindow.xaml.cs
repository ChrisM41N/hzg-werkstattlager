using System.Windows;
using WerkstattlagerAPI.Models;
using WerkstattlagerViewLogic.ViewModels;

namespace WerkstattlagerUI.Views
{
    public partial class CreateCategoryWindow : Window
    {
        private readonly CategoryOverview _categoryOverview;
        public CreateCategoryWindow(CategoryOverview categoryOverview)
        {
            InitializeComponent();
            _categoryOverview = categoryOverview;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (IdField.Text.Length == 1)
            {
                var category = new Category
                {
                    Id = IdField.Text.ToCharArray().First(),
                    Description = DescriptionField.Text,
                };
                _ = _categoryOverview.CreateCategory(category);
                Close();
            }
        }
    }
}
