using AutoMapper;

namespace AltinnDesktopTool.ViewModel.MapperProfiles
{
    public class SearchMapperProfile : Profile
    {
        public SearchMapperProfile()
        {
            CreateMap<RestClient.DTO.Organization, Model.OrganizationModel>();
            CreateMap<RestClient.DTO.OfficialContact, Model.OfficialContactModel>();
            CreateMap<RestClient.DTO.PersonalContact, Model.PersonalContactModel>();
        }
    }
}
