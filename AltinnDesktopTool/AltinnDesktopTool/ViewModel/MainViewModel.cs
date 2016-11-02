using System.ComponentModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace AltinnDesktopTool.ViewModel
{
    using System.Windows;

    using Configuration;

    using MahApps.Metro;

    using Utils.PubSub;

    /// <summary>
    /// ViewModel for MainView
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>        
        public MainViewModel()
        {
            PubSub<string>.RegisterEvent(EventNames.EnvironmentChangedEvent, this.EnvironmentChangedEventHandler);
            this.ClosingWindowCommand = new RelayCommand<CancelEventArgs>(this.ClosingWindowCommandHandler);
        }

        /// <summary>
        /// Gets or sets Closing window command
        /// </summary>
        public RelayCommand<CancelEventArgs> ClosingWindowCommand { get; set; }

        private void ClosingWindowCommandHandler(CancelEventArgs obj)
        {
            ViewModelLocator.Cleanup();
        }
        
        private void EnvironmentChangedEventHandler(object sender, PubSubEventArgs<string> args)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(EnvironmentConfigurationManager.ActiveEnvironmentConfiguration.ThemeName), ThemeManager.GetAppTheme("BaseLight"));
        }
    }
}