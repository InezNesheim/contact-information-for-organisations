using System.Windows;
using MahApps.Metro.Controls.Dialogs;

namespace AltinnDesktopTool.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TopView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            await this.ShowMessageAsync(View.Resources.HeaderText, View.Resources.InfoText);
        }
    }
}
