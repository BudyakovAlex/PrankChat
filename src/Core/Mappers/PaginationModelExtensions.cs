using PrankChat.Mobile.Core.Models.Api.Base;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class PaginationModelExtensions
    {
        public static PaginationModel<TDataModel> Map<TDataModel, TApiModel>(this BaseBundleApiModel<TApiModel> baseBundleApiModel, Func<TApiModel, TDataModel> transformFunc)
            where TDataModel : class
            where TApiModel : class
        {
            var mappedModels = baseBundleApiModel?.Data?.Select(transformFunc)?.ToList() ?? new List<TDataModel>();
            var paginationData = baseBundleApiModel?.Meta?.FirstOrDefault();
            var totalItemsCount = paginationData?.Value?.Total ?? mappedModels.Count;
            return new PaginationModel<TDataModel>(mappedModels, totalItemsCount);
        }
    }
}