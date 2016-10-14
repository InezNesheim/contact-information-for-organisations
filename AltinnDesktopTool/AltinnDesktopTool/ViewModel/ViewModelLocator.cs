/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:AltinnDesktopTool"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using AltinnDesktopTool.ViewModel.Mappers;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using log4net;
using Microsoft.Practices.ServiceLocation;

namespace AltinnDesktopTool.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Logging
            SimpleIoc.Default.Register(() => LogManager.GetLogger(GetType())); // ILog
            log4net.Config.XmlConfigurator.Configure();

            // AutoMapper
            SimpleIoc.Default.Register(RunCreateMaps); // IMapper

            // View models
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SearchOrganizationInformationViewModel>();
            SimpleIoc.Default.Register<SearchResultViewModel>();
        }
        
        public ViewModelBase Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public SearchOrganizationInformationViewModel SearchOrganizationInformationViewModel => ServiceLocator.Current.GetInstance<SearchOrganizationInformationViewModel>();
        public SearchResultViewModel SearchResultViewModel => ServiceLocator.Current.GetInstance<SearchResultViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        private static IMapper RunCreateMaps()
        {
            // Add profiles here
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<SearchMapperProfile>();
            });

            return config.CreateMapper();
        }
    }
}