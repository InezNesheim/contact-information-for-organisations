using AltinnDesktopTool.ViewModel.MapperProfiles;

using AutoMapper;

namespace AltinnDesktopTool.Utils.Helpers
{
    public class AutoMapperHelper
    {
        public static IMapper RunCreateMaps()
        {
            // Add profiles here
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<SearchMapperProfile>();
            });

            return Mapper.Configuration.CreateMapper();
        }
    }
}
