using AutoMapper;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Models.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationDataModel, UserRegistrationApiModel>().ReverseMap();
            CreateMap<CreateOrderDataModel, CreateOrderApiModel>().ReverseMap();
        }
    }
}
