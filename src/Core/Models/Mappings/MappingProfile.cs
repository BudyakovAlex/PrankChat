﻿using System.Linq;
using AutoMapper;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.Network;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
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
                .ForPath(dest => dest.Customer.Data, opt => opt.MapFrom(src => src.Customer))
                .ReverseMap();
            CreateMap<PaginationInfoDataModel, PaginationInfoApiModel>().ReverseMap();
            CreateMap<CreateOrderDataModel, CreateOrderApiModel>().ReverseMap();
            CreateMap<UserDataModel, UserApiModel>()
                .ForPath(dest => dest.Document.Data, opt => opt.MapFrom(src => src.Document))
                .ReverseMap();
            CreateMap<OrderDataModel, OrderApiModel>()
                .ForPath(dest => dest.Customer.Data, opt => opt.MapFrom(src => src.Customer))
                .ForPath(dest => dest.Executor.Data, opt => opt.MapFrom(src => src.Executor))
                .ForPath(dest => dest.Video.Data, opt => opt.MapFrom(src => src.Video))
                .ReverseMap();
            CreateMap<UserUpdateProfileDataModel, UserUpdateProfileApiModel>().ReverseMap();
            CreateMap<ArbitrationOrderDataModel, ArbitrationOrderApiModel>()
                .ForPath(dest => dest.Customer.Data, opt => opt.MapFrom(src => src.Customer))
                .ForPath(dest => dest.Executor.Data, opt => opt.MapFrom(src => src.Executor))
                .ForPath(dest => dest.Video.Data, opt => opt.MapFrom(src => src.Video))
                .ReverseMap();
            CreateMap<PaymentDataModel, PaymentApiModel>().ReverseMap();
            CreateMap<CompetitionDataModel, CompetitionApiModel>().ReverseMap();
            CreateMap<CompetitionResultDataModel, CompetitionResultApiModel>()
                .ForPath(dest => dest.Video.Data, opt => opt.MapFrom(src => src.Video))
                .ForPath(dest => dest.User.Data, opt => opt.MapFrom(src => src.User))
                .ReverseMap();
            CreateMap<NotificationDataModel, NotificationApiModel>()
                .ForPath(dest => dest.RelatedOrder.Data, opt => opt.MapFrom(src => src.RelatedOrder))
                .ForPath(dest => dest.RelatedUser.Data, opt => opt.MapFrom(src => src.RelatedUser))
                .ForPath(dest => dest.RelatedVideo.Data, opt => opt.MapFrom(src => src.RelatedVideo))
                .ForPath(dest => dest.RelationTransaction.Data, opt => opt.MapFrom(src => src.RelationTransaction))
                .ReverseMap();
            CreateMap<CommentDataModel, CommentApiModel>()
                .ForPath(dest => dest.User.Data, opt => opt.MapFrom(src => src.User))
                .ReverseMap();
            CreateMap<TransactionDataModel, TransactionApiModel>().ReverseMap();
            CreateMap<ProblemDetailsDataModel, ProblemDetailsApiModel>()
                .ForPath(dest => dest.Message, opt => opt.MapFrom(src => src.MessageServerError))
                .ReverseMap();
            CreateMap<RecoverPasswordResultDataModel, RecoverPasswordResultApiModel>().ReverseMap();
            CreateMap<DocumentDataModel, DocumentApiModel>().ReverseMap();
            CreateMap<CardDataModel, CardApiModel>().ReverseMap();
            CreateMap<WithdrawalDataModel, WithdrawalApiModel>().ReverseMap();

            CreateMap<ArbitrationValueType, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<GenderType, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, OrderStatusType>().ConvertUsing(src => src.ToEnum<OrderStatusType>());
        }
    }
}
