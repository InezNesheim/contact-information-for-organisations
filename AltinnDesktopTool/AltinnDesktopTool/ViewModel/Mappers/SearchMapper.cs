using AutoMapper;

namespace AltinnDesktopTool.ViewModel.Mappers
{
    public class SearchMapper : IAmAMapper
    {
        public void CreateMaps()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<RestClient.DTO.Organization, Model.OrganizationModel>();
            });
        }
    }
}
