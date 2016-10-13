using GalaSoft.MvvmLight;
using log4net;

namespace AltinnDesktopTool.ViewModel
{
    public class SearchResultViewModel : ViewModelBase
    {
        private readonly ILog _logger;

        public SearchResultViewModel(ILog logger)
        {
            _logger = logger;
        }
    }
}
