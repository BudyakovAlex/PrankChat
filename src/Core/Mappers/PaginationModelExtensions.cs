using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Api.Base;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class PaginationModelExtensions
    {
        public static PaginationModel<CommentDataModel> Map(this BaseBundleApiModel<CommentApiModel> baseBundleApiModel)
        {
            var mappedModels = baseBundleApiModel?.Data.Select(comment => comment.Map()).ToList();
            var paginationData = baseBundleApiModel?.Meta?.FirstOrDefault();
            var totalItemsCount = paginationData?.Value?.Total ?? mappedModels.Count;
            return new PaginationModel<CommentDataModel>(mappedModels, totalItemsCount);
        }

        public static PaginationModel<VideoDataModel> Map(this BaseBundleApiModel<VideoApiModel> baseBundleApiModel)
        {
            var mappedModels = baseBundleApiModel?.Data.Select(video => video.Map()).ToList();
            var paginationData = baseBundleApiModel?.Meta?.FirstOrDefault();
            var totalItemsCount = paginationData?.Value?.Total ?? mappedModels.Count;
            return new PaginationModel<VideoDataModel>(mappedModels, totalItemsCount);
        }

        public static PaginationModel<CompetitionDataModel> Map(this BaseBundleApiModel<CompetitionApiModel> baseBundleApiModel)
        {
            var mappedModels = baseBundleApiModel?.Data.Select(comp => comp.Map()).ToList();
            var paginationData = baseBundleApiModel?.Meta?.FirstOrDefault();
            var totalItemsCount = paginationData?.Value?.Total ?? mappedModels.Count;
            return new PaginationModel<CompetitionDataModel>(mappedModels, totalItemsCount);
        }

        public static PaginationModel<UserDataModel> Map(this BaseBundleApiModel<UserApiModel> baseBundleApiModel)
        {
            var mappedModels = baseBundleApiModel?.Data.Select(user => user.Map()).ToList();
            var paginationData = baseBundleApiModel?.Meta?.FirstOrDefault();
            var totalItemsCount = paginationData?.Value?.Total ?? mappedModels.Count;
            return new PaginationModel<UserDataModel>(mappedModels, totalItemsCount);
        }

    }
}
