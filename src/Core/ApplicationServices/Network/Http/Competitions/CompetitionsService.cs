using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Abstract;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Api.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Competitions
{
    public class CompetitionsService : BaseRestService, ICompetitionsService
    {
        private readonly ISettingsService _settingsService;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public CompetitionsService(ISettingsService settingsService,
                                   IAuthorizationService authorizeService,
                                   IMvxLogProvider logProvider,
                                   IMvxMessenger messenger,
                                   ILogger logger) : base(settingsService, authorizeService, logProvider, messenger, logger)
        {
            _settingsService = settingsService;
            _messenger = messenger;
            _log = logProvider.GetLogFor<CompetitionsService>();

            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress,
                                     configuration.ApiVersion,
                                     settingsService,
                                     _log,
                                     logger,
                                     messenger);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
        }

        public async Task<List<CompetitionResultApiModel>> GetCompetitionResultsAsync(int id)
        {
            var bundle = await _client.GetAsync<BaseBundleApiModel<CompetitionResultApiModel>>($"competition/{id}/results");
            return bundle?.Data;
        }

        public async Task<List<CompetitionResultApiModel>> GetCompetitionRatingsAsync(int id)
        {
            var bundle = await _client.GetAsync<BaseBundleApiModel<CompetitionResultApiModel>>($"competition/{id}/rating");
            return bundle?.Data;
        }

        public async Task<BaseBundleApiModel<VideoApiModel>> GetCompetitionVideosAsync(int competitionId, int page, int pageSize)
        {
            BaseBundleApiModel<VideoApiModel> videoMetadataBundle;
            if (_settingsService.User == null)
            {
                videoMetadataBundle = await _client.UnauthorizedGetAsync<BaseBundleApiModel<VideoApiModel>>($"competition/{competitionId}/videos?page={page}&items_per_page={pageSize}", false, IncludeType.User);
            }
            else
            {
                videoMetadataBundle = await _client.GetAsync<BaseBundleApiModel<VideoApiModel>>($"competition/{competitionId}/videos?page={page}&items_per_page={pageSize}", false, IncludeType.User);
            }

            return videoMetadataBundle;
        }

        public Task<BaseBundleApiModel<CompetitionApiModel>> GetCompetitionsAsync(int page, int pageSize)
        {
            var endpoint = $"competitions?page={page}&items_per_page={pageSize}";
            var data = _client.GetAsync<BaseBundleApiModel<CompetitionApiModel>>(endpoint);
            return data;
        }

        public Task<CompetitionApiModel> CompetitionJoinAsync(int id)
        {
            return _client.PostAsync<CompetitionApiModel>($"competitions/{id}/join", true);
        }
    }
}