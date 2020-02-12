using System.Linq;
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
            CreateMap<VideoDataModel, VideoApiModel> ()
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
            CreateMap<UserUpdateProfileDataModel, UserUpdateProfileApiModel>().ReverseMap();
            CreateMap<RatingOrderDataModel, RatingOrderApiModel>()
                .ForPath(dest => dest.Customer.Data, opt => opt.MapFrom(src => src.Customer))
                .ReverseMap();
            CreateMap<GenderType, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<ArbitrationValueType, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<NotificationDataModel, NotificationApiModel>()
                .ForPath(dest => dest.RelatedOrder.Data, opt => opt.MapFrom(src => src.RelatedOrder))
                .ForPath(dest => dest.RelatedUser.Data, opt => opt.MapFrom(src => src.RelatedUser))
                .ForPath(dest => dest.RelatedVideo.Data, opt => opt.MapFrom(src => src.RelatedVideo))
                .ForPath(dest => dest.RelationTransaction.Data, opt => opt.MapFrom(src => src.RelationTransaction))
                .ReverseMap();
            CreateMap<TransactionDataModel, TransactionApiModel>().ReverseMap();
        }
    }
}
