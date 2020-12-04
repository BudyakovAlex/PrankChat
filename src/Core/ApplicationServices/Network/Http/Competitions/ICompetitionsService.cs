using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Api.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Competitions
{
    public interface ICompetitionsService
    {
        Task<BaseBundleApiModel<VideoApiModel>> GetCompetitionVideosAsync(int competitionId, int page, int pageSize);

        Task<BaseBundleApiModel<CompetitionApiModel>> GetCompetitionsAsync(int page, int pageSize);

        Task<List<CompetitionResultApiModel>> GetCompetitionResultsAsync(int id);

        Task<List<CompetitionResultApiModel>> GetCompetitionRatingsAsync(int id);
    }
}