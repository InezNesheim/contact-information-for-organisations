using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AltinnDesktopTool.View
{
    /// <summary>
    /// Interaction logic for SearchResultView.xaml
    /// </summary>
    public partial class SearchResultView
    {
        /// <summary>
        /// Initializes a new instance of the SearchResultView class.
        /// </summary>
        public SearchResultView()
        {
            this.InitializeComponent();
        }

        private void UIElement_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.IsChecked ?? false)
            {
                checkBox.SetCurrentValue(CheckBox.IsCheckedProperty, false);
            }
            else
            {
                checkBox.SetCurrentValue(CheckBox.IsCheckedProperty, true);
            }

            e.Handled = true;
        }
    }
}
