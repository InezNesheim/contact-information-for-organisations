namespace AltinnDesktopTool.View
{
    using System.Windows.Controls;

    using Configuration;

    /// <summary>
    /// Interaction logic for TopView.xaml
    /// </summary>
    public partial class Footer
    {
        public Footer()
        {
            var configItems = EnvironmentConfigurationManager.EnvironmentConfigurations;
            this.InitializeComponent();
            this.configCombo.ItemsSource = configItems;
            this.configCombo.DisplayMemberPath = "Name";
            this.configCombo.SelectedValue = configItems.Find(c => c.Name == "PROD");
        }

        private void ConfigCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                        
        }
    }
}
