using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Abstract;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Competitions
{
    public class CompetitionsService : BaseRestService, ICompetitionsService
    {
        private readonly IUserSessionProvider _userSessionProvider;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public CompetitionsService(
            IUserSessionProvider userSessionProvider,
            IAuthorizationService authorizeService,
            IMvxLogProvider logProvider,
            IMvxMessenger messenger) : base(userSessionProvider, authorizeService, logProvider, messenger)
        {
            _userSessionProvider = userSessionProvider;
            _messenger = messenger;
            _log = logProvider.GetLogFor<CompetitionsService>();

            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(
                configuration.BaseAddress,
                configuration.ApiVersion,
                userSessionProvider,
                _log,
                messenger);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
        }

        public async Task<List<CompetitionResultDto>> GetCompetitionResultsAsync(int id)
        {
            var bundle = await _client.GetAsync<BaseBundleDto<CompetitionResultDto>>($"competition/{id}/results");
            return bundle?.Data;
        }

        public async Task<List<CompetitionResultDto>> GetCompetitionRatingsAsync(int id)
        {
            var bundle = await _client.GetAsync<BaseBundleDto<CompetitionResultDto>>($"competition/{id}/rating");
            return bundle?.Data;
        }

        public async Task<BaseBundleDto<VideoDto>> GetCompetitionVideosAsync(int competitionId, int page, int pageSize)
        {
            BaseBundleDto<VideoDto> videoMetadataBundle;
            if (_userSessionProvider.User == null)
            {
                videoMetadataBundle = await _client.UnauthorizedGetAsync<BaseBundleDto<VideoDto>>($"competition/{competitionId}/videos?page={page}&items_per_page={pageSize}", false, IncludeType.User);
            }
            else
            {
                videoMetadataBundle = await _client.GetAsync<BaseBundleDto<VideoDto>>($"competition/{competitionId}/videos?page={page}&items_per_page={pageSize}", false, IncludeType.User);
            }

            return videoMetadataBundle;
        }

        public Task<BaseBundleDto<CompetitionDto>> GetCompetitionsAsync(int page, int pageSize)
        {
            var endpoint = $"competitions?page={page}&items_per_page={pageSize}";
            var data = _client.GetAsync<BaseBundleDto<CompetitionDto>>(endpoint);
            return data;
        }

        public async Task<CompetitionDto> CompetitionJoinAsync(int id)
        {
            var response = await _client.PostAsync<ResponseDto<CompetitionDto>>($"competitions/{id}/join", true);
            return response?.Data;
        }
    }
}