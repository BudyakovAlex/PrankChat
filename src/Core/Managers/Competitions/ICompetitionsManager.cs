using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Competitions
{
    public interface ICompetitionsManager
    {
        Task<Pagination<Models.Data.Video>> GetCompetitionVideosAsync(int competitionId, int page, int pageSize);

        Task<Pagination<Competition>> GetCompetitionsAsync(int page, int pageSize);

        Task<List<CompetitionResult>> GetCompetitionResultsAsync(int id);

        Task<List<CompetitionResult>> GetCompetitionRatingsAsync(int id);

        Task<Competition> CompetitionJoinAsync(int id);

        Task<Competition> CancelCompetitionAsync(int id);
    }
}