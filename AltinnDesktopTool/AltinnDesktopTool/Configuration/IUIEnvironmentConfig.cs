using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltinnDesktopTool.Configuration
{
    //TODO:: define UI settings
    public interface IUIEnvironmentConfig
    {
        string Name { get; set; }
        string ThemeName { get; set; }
    }
}
