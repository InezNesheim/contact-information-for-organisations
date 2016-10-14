using AutoMapper;

namespace AltinnDesktopTool.ViewModel.MapperProfiles
{
    public class SearchMapperProfile : Profile
    {
        public SearchMapperProfile()
        {
            CreateMap<RestClient.DTO.Organization, Model.OrganizationModel>();
        }
    }
}
