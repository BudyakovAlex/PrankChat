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

        public Task<List<CompetitionResultDataModel>> GetCompetitionResultsAsync(int id)
        {
            return _competitionsService.GetCompetitionResultsAsync(id);
        }

        public Task<List<CompetitionResultDataModel>> GetCompetitionRatingsAsync(int id)
        {
            return _competitionsService.GetCompetitionRatingsAsync(id);
        }

        public Task<PaginationModel<VideoDataModel>> GetCompetitionVideosAsync(int competitionId, int page, int pageSize)
        {
            return _competitionsService.GetCompetitionVideosAsync(competitionId, page, pageSize);
        }

        public Task<PaginationModel<CompetitionDataModel>> GetCompetitionsAsync(int page, int pageSize)
        {
            return _competitionsService.GetCompetitionsAsync(page, pageSize);
        }
    }
}