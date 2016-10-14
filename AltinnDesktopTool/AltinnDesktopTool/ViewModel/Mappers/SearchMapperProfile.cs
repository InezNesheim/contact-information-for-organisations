using AutoMapper;

namespace AltinnDesktopTool.ViewModel.Mappers
{
    public class SearchMapperProfile : Profile
    {
        public SearchMapperProfile()
        {
            CreateMap<RestClient.DTO.Organization, Model.OrganizationModel>();
        }
    }
}
