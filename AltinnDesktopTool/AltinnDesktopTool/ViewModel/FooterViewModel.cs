using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace AltinnDesktopTool.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    using AltinnDesktopTool.Configuration;
    using AltinnDesktopTool.Model;
    using AltinnDesktopTool.Utils.Helpers;
    using AltinnDesktopTool.Utils.PubSub;

    using log4net;

    using MahApps.Metro;

    using Microsoft.Practices.ServiceLocation;

    using RestClient;

    public class FooterViewModel : ViewModelBase
    {
        private List<EnvironmentConfiguration> configItems;

        private ObservableCollection<string> environmentNames = new ObservableCollection<string>();

        private string selectedEnvironment;

        public string SelectedEnvironment
        {
            get
            {
                return this.selectedEnvironment;
            }
            set
            {
                this.selectedEnvironment = value;
            }
        }

        public ObservableCollection<string> EnvironmentNames
        {
            get
            {
                return this.environmentNames;
            }
            set
            {
                this.environmentNames = value;
            }
        }

        public ICommand ChangeEnvironmentCommand { get; private set; }
        public event PubSubEventHandler<string> EnvironmentChangedEventHandler;


        public FooterViewModel()
        {
            ChangeEnvironmentCommand = new RelayCommand(ChangeEnvironmentHandler);

            configItems = EnvironmentConfigurationManager.EnvironmentConfigurations;
            foreach (var environmentConfiguration in this.configItems)
            {
                EnvironmentNames.Add(environmentConfiguration.Name);
            }

            this.SelectedEnvironment = EnvironmentNames.Single(c => c == "PROD");

            PubSub<string>.AddEvent(EventNames.EnvironmentChangedEvent, this.EnvironmentChangedEventHandler);

        }

        public void ChangeEnvironmentHandler()
        {            
            PubSub<string>.RaiseEvent(EventNames.EnvironmentChangedEvent, this, new PubSubEventArgs<string>(this.selectedEnvironment));
        }
    }
}
