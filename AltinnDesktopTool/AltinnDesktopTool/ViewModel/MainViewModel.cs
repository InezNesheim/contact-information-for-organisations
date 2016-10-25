using GalaSoft.MvvmLight;

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
        // ReSharper disable once EmptyConstructor
        public MainViewModel()
        {            
            PubSub<string>.RegisterEvent(EventNames.EnvironmentChangedEvent, this.EnvironmentChangedEventHandler);
        }

        private void EnvironmentChangedEventHandler(object sender, PubSubEventArgs<string> args)
        {            
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(EnvironmentConfigurationManager.ActiveEnvironmentConfiguration.ThemeName), ThemeManager.GetAppTheme("BaseLight"));
        }
    }
}