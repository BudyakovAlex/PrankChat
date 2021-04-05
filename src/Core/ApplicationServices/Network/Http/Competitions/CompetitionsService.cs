﻿using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Competitions
{
    public class CompetitionsService : ICompetitionsService
    {
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly HttpClient _client;

        public CompetitionsService(
            IUserSessionProvider userSessionProvider,
            IEnvironmentConfigurationProvider environmentConfigurationProvider,
            IMvxLogProvider logProvider,
            IMvxMessenger messenger)
        {
            _userSessionProvider = userSessionProvider;
            var log = logProvider.GetLogFor<CompetitionsService>();

            var environment = environmentConfigurationProvider.Environment;

            _client = new HttpClient(
                environment.ApiUrl,
                environment.ApiVersion,
                userSessionProvider,
                log,
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