using AltinnDesktopTool.Model;
using GalaSoft.MvvmLight;

namespace AltinnDesktopTool.ViewModel
{
    public class AltinnViewModelBase : ViewModelBase
    {
        public bool IsBusy { get; set; }
        public virtual ModelBase Model { get; set; }
    }
}
