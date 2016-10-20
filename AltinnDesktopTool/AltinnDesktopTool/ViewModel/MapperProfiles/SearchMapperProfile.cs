using AutoMapper;

namespace AltinnDesktopTool.ViewModel.MapperProfiles
{
    public class SearchMapperProfile : Profile
    {
        public SearchMapperProfile()
        {
            this.CreateMap<RestClient.DTO.Organization, Model.OrganizationModel>();
            this.CreateMap<RestClient.DTO.OfficialContact, Model.OfficialContactModel>();
            this.CreateMap<RestClient.DTO.PersonalContact, Model.PersonalContactModel>();
        }
    }
}
