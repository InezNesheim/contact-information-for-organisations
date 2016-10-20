namespace AltinnDesktopTool.View
{
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
    }
}
