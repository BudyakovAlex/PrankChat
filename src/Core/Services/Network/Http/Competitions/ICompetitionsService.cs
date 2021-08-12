using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Services.Network.Http.Competitions
{
    public interface ICompetitionsService
    {
        Task<BaseBundleDto<VideoDto>> GetCompetitionVideosAsync(int competitionId, int page, int pageSize);

        Task<BaseBundleDto<CompetitionDto>> GetCompetitionsAsync(int page, int pageSize);

        Task<List<CompetitionResultDto>> GetCompetitionResultsAsync(int id);

        Task<List<CompetitionResultDto>> GetCompetitionRatingsAsync(int id);

        Task<CompetitionDto> CompetitionJoinAsync(int id);
    }
}