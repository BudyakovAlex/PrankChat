using PrankChat.Mobile.Core.Data.Dtos.Base;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class PaginationExtensions
    {
        public static Pagination<TModel> Map<TModel, TDto>(this BaseBundleDto<TDto> baseBundle, Func<TDto, TModel> transformFunc)
            where TModel : class
            where TDto : class
        {
            var mappedModels = baseBundle?.Data?.Select(transformFunc)?.ToList() ?? new List<TModel>();
            var pagination = baseBundle?.Meta?.FirstOrDefault();
            var totalItemsCount = pagination?.Value?.Total ?? mappedModels.Count;
            return new Pagination<TModel>(mappedModels, totalItemsCount);
        }
    }
}