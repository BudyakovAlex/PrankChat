using PrankChat.Mobile.Core.Data.Models;
using PrankChat.Mobile.Core.Data.Models.Competitions;
using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Services.Network.Http.Competitions;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<CompetitionResult>> GetCompetitionResultsAsync(int id)
        {
            var response = await _competitionsService.GetCompetitionResultsAsync(id);
            return response.Select(comp => comp.Map()).ToList();
        }

        public async Task<List<CompetitionResult>> GetCompetitionRatingsAsync(int id)
        {
            var response = await _competitionsService.GetCompetitionRatingsAsync(id);
            return response?.Select(comp => comp.Map()).ToList() ?? new List<CompetitionResult>();
        }

        public async Task<Pagination<Models.Data.Video>> GetCompetitionVideosAsync(int competitionId, int page, int pageSize)
        {
            var response = await _competitionsService.GetCompetitionVideosAsync(competitionId, page, pageSize);
            return response.Map(item => item.Map());
        }

        public async Task<Pagination<Competition>> GetCompetitionsAsync(int page, int pageSize)
        {
            var response = await _competitionsService.GetCompetitionsAsync(page, pageSize);
            return response.Map(item => item.Map());
        }

        public async Task<Competition> CompetitionJoinAsync(int id)
        {
            var response = await _competitionsService.CompetitionJoinAsync(id);
            return response.Map();
        }

        public async Task<Competition> CancelCompetitionAsync(int id)
        {
            var response = await _competitionsService.CancelCompetitionAsync(id);
            return response.Map();
        }

        public async Task<CompetitionStatistics> GetCompetitionStatisticsAsync(int id)
        {
            var response = await _competitionsService.GetCompetitionStatisticsAsync(id);
            return response.Map();
        }

        public async Task<Competition> CreateCompetitionAsync(CompetitionCreationForm competition)
        {
            var dto = competition.Map();
            var response = await _competitionsService.CreateCompetitionAsync(dto);
            return response.Map();
        }
    }
}