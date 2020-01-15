using AutoMapper;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationDataModel, UserRegistrationApiModel>().ReverseMap();
            CreateMap<VideoMetadataBundleDataModel, VideoMetadataBundleApiModel>().ReverseMap();
            CreateMap<VideoMetadataDataModel, VideoMetadataApiModel>().ReverseMap();
            CreateMap<PaginationInfoDataModel, PaginationInfoApiModel>().ReverseMap();
            CreateMap<OrderDataModel, OrderApiModel>().ReverseMap();
            CreateMap<CreateOrderDataModel, CreateOrderApiModel>().ReverseMap();
            CreateMap<UserDataModel, UserApiModel>().ReverseMap();
            CreateMap<GenderType, string>().ConvertUsing(src => src.ToString().ToLower());
        }
    }
}
