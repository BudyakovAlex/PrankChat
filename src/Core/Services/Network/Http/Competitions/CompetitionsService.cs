using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.Providers.UserSession;

namespace PrankChat.Mobile.Core.Services.Network.Http.Competitions
{
    public class CompetitionsService : ICompetitionsService
    {
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly HttpClient _client;

        public CompetitionsService(
            IUserSessionProvider userSessionProvider,
            IEnvironmentConfigurationProvider environmentConfigurationProvider,
            IMvxMessenger messenger)
        {
            _userSessionProvider = userSessionProvider;

            var environment = environmentConfigurationProvider.Environment;

            _client = new HttpClient(
                environment.ApiUrl,
                environment.ApiVersion,
                userSessionProvider,
                this.Logger(),
                messenger);
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
            var data = _client.GetAsync<BaseBundleDto<CompetitionDto>>(endpoint, includes: new[] { IncludeType.Customer });
            return data;
        }

        public async Task<CompetitionDto> CompetitionJoinAsync(int id)
        {
            var response = await _client.PostAsync<ResponseDto<CompetitionDto>>($"competitions/{id}/join", true);
            return response?.Data;
        }

        public async Task<CompetitionDto> CancelCompetitionAsync(int id)
        {
            var data = await _client.PostAsync<ResponseDto<CompetitionDto>>($"competitions/{id}/cancel", false);
            return data?.Data;
        }

        public async Task<CompetitionStatisticsDto> GetCompetitionStatisticsAsync(int id)
        {
            var data = await _client.PostAsync<ResponseDto<CompetitionStatisticsDto>>($"competitions/{id}/statistics", false);
            return data?.Data;
        }
    }
}