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
            CreateMap<VideoMetadataApiModel, VideoMetadataDataModel>().ForMember(c => c.User, options => options.MapFrom(x => x.User["data"])).ReverseMap();
            CreateMap<PaginationInfoDataModel, PaginationInfoApiModel>().ReverseMap();
            CreateMap<CreateOrderDataModel, CreateOrderApiModel>().ReverseMap();
            CreateMap<UserDataModel, UserApiModel>().ReverseMap();
            CreateMap<OrderDataModel, OrderApiModel>()
                .ForPath(dest => dest.Customer.Data, opt => opt.MapFrom(src => src.Customer))
                .ForPath(dest => dest.Executor.Data, opt => opt.MapFrom(src => src.Executor))
                .ForPath(dest => dest.Video.Data, opt => opt.MapFrom(src => src.Video))
                .ReverseMap();
            CreateMap<GenderType, string>().ConvertUsing(src => src.ToString().ToLower());
        }
    }
}
