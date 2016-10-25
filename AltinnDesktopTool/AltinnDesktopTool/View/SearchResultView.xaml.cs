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
        public SearchResultView()
        {
            this.InitializeComponent();      
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // the original source is what was clicked.  For example 
                // a button.
                DependencyObject dep = (DependencyObject)e.OriginalSource;

                // iteratively traverse the visual tree upwards looking for
                // the clicked row.
                while ((dep != null) && !(dep is DataGridRow))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }

                // if we found the clicked row
                if (dep != null && dep is DataGridRow)
                {
                    // get the row
                    DataGridRow row = (DataGridRow)dep;

                    // change the details visibility
                    if (row.DetailsVisibility == Visibility.Collapsed)
                    {
                        row.DetailsVisibility = Visibility.Visible;
                    }
                    else
                    {
                        row.DetailsVisibility = Visibility.Collapsed;
                    }
                }
            }
            catch (System.Exception)
            {
            }
        }

        private void UIElement_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CheckBox checkBox = (CheckBox) sender;
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
