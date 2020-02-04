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
            CreateMap<VideoMetadataDataModel, VideoMetadataApiModel> ()
                .ForPath(dest => dest.User.Data, opt => opt.MapFrom(src => src.User))
                .ReverseMap();
            CreateMap<PaginationInfoDataModel, PaginationInfoApiModel>().ReverseMap();
            CreateMap<CreateOrderDataModel, CreateOrderApiModel>().ReverseMap();
            CreateMap<UserDataModel, UserApiModel>().ReverseMap();
            CreateMap<OrderDataModel, OrderApiModel>()
                .ForPath(dest => dest.Customer.Data, opt => opt.MapFrom(src => src.Customer))
                .ForPath(dest => dest.Executor.Data, opt => opt.MapFrom(src => src.Executor))
                .ForPath(dest => dest.Video.Data, opt => opt.MapFrom(src => src.Video))
                .ReverseMap();
            CreateMap<GenderType, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<UserUpdateProfileDataModel, UserUpdateProfileApiModel>().ReverseMap();
            CreateMap<RatingOrderDataModel, RatingOrderApiModel>()
                .ForPath(dest => dest.Customer.Data, opt => opt.MapFrom(src => src.Customer))
                .ReverseMap();
            CreateMap<NotificationMetadataDataModel, NotificationMetadataApiModel>().ReverseMap();
            CreateMap<TransactionDataModel, TransactionApiModel>().ReverseMap();
        }
    }
}
