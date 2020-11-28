using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Competitions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            return await _competitionsService.GetCompetitionResultsAsync(id);
        }

        public async Task<List<CompetitionResultDataModel>> GetCompetitionRatingsAsync(int id)
        {
            return await _competitionsService.GetCompetitionRatingsAsync(id);
        }

        public async Task<PaginationModel<VideoDataModel>> GetCompetitionVideosAsync(int competitionId, int page, int pageSize)
        {
            return await _competitionsService.GetCompetitionVideosAsync(competitionId, page, pageSize);
        }

        public async Task<PaginationModel<CompetitionDataModel>> GetCompetitionsAsync(int page, int pageSize)
        {
            return await _competitionsService.GetCompetitionsAsync(page, pageSize);
        }
    }
}