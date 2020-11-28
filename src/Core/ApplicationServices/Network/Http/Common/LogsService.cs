﻿using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using Plugin.DeviceInfo;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Abstract;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Models.Api.Base;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Common
{
    public class LogsService : BaseRestService, ILogsService
    {
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public LogsService(
            ISettingsService settingsService,
            IAuthorizationService authorizeService,
            IMvxLogProvider logProvider,
            IMvxMessenger messenger,
            ILogger logger) : base(settingsService, authorizeService, logProvider, messenger, logger)
        {
            _messenger = messenger;
            _log = logProvider.GetLogFor<LogsService>();

            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress,
                                     configuration.ApiVersion,
                                     settingsService,
                                     _log,
                                     logger,
                                     messenger);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
        }

        public Task<bool> SendLogsAsync(string filePath)
        {
            return _client.PostFileAsync("application/log/mobile", "file", filePath, false, new KeyValuePair<string, string>("device_id", CrossDeviceInfo.Current.Id));
        }

        private PaginationModel<TDataModel> CreatePaginationResult<TApiModel, TDataModel>(BaseBundleApiModel<TApiModel> data)
            where TDataModel : class
            where TApiModel : class
        {
            var mappedModels = MappingConfig.Mapper.Map<List<TDataModel>>(data?.Data ?? new List<TApiModel>());
            var paginationData = data?.Meta?.FirstOrDefault();
            var totalItemsCount = paginationData?.Value?.Total ?? mappedModels.Count;
            return new PaginationModel<TDataModel>(mappedModels, totalItemsCount);
        }
    }
}