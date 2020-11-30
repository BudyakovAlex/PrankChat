using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Competitions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrankChat.Mobile.Core.Mappers;
using System.Linq;
using PrankChat.Mobile.Core.Models.Api.Base;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Models.Api;

namespace PrankChat.Mobile.Core.Managers.Competitions
{
    public class CompetitionsManager : ICompetitionsManager
    {
        private readonly ICompetitionsService _competitionsService;

        public CompetitionsManager(ICompetitionsService competitionsService)
        {
            _competitionsService = competitionsService;
        }

        public async Task<List<CompetitionResultDataModel>> GetCompetitionResultsAsync(int id)
        {
            var response = await _competitionsService.GetCompetitionResultsAsync(id);
            return response.Select(comp => comp.Map()).ToList();
        }

        public async Task<List<CompetitionResultDataModel>> GetCompetitionRatingsAsync(int id)
        {
            var response = await _competitionsService.GetCompetitionRatingsAsync(id);
            return response.Select(comp => comp.Map()).ToList();
        }

        public async Task<PaginationModel<VideoDataModel>> GetCompetitionVideosAsync(int competitionId, int page, int pageSize)
        {
            var response = await _competitionsService.GetCompetitionVideosAsync(competitionId, page, pageSize);
            return response.Map();
        }

        public async Task<PaginationModel<CompetitionDataModel>> GetCompetitionsAsync(int page, int pageSize)
        {
            var response = await _competitionsService.GetCompetitionsAsync(page, pageSize);
            return response.Map();
        }
    }
}