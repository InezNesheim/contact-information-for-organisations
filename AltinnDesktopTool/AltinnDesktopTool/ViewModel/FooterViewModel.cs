using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using AltinnDesktopTool.Configuration;
using AltinnDesktopTool.Utils.PubSub;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace AltinnDesktopTool.ViewModel
{
    public class FooterViewModel : ViewModelBase
    {
        /// <summary>
        /// Environment changed event
        /// </summary>
        public event PubSubEventHandler<string> EnvironmentChangedEventHandler;

        public string SelectedEnvironment { get; set; }

        public ObservableCollection<string> EnvironmentNames { get; set; } = new ObservableCollection<string>();

        public ICommand ChangeEnvironmentCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FooterViewModel"/> class.
        /// ViewModel for Footer view
        /// </summary>
        public FooterViewModel()
        {
            this.ChangeEnvironmentCommand = new RelayCommand(this.ChangeEnvironmentHandler);

            this.EnvironmentNames = new ObservableCollection<string>(EnvironmentConfigurationManager.EnvironmentConfigurations.Select(c => c.Name).ToList());
            this.SelectedEnvironment = EnvironmentConfigurationManager.ActiveEnvironmentConfiguration.Name;

            PubSub<string>.AddEvent(EventNames.EnvironmentChangedEvent, this.EnvironmentChangedEventHandler);
        }

        /// <summary>
        /// Event
        /// </summary>
        public void ChangeEnvironmentHandler()
        {
            EnvironmentConfigurationManager.ActiveEnvironmentConfiguration = EnvironmentConfigurationManager.EnvironmentConfigurations.Single(c => c.Name == this.SelectedEnvironment);
            PubSub<string>.RaiseEvent(EventNames.EnvironmentChangedEvent, this, new PubSubEventArgs<string>(this.SelectedEnvironment));
        }
    }
}
