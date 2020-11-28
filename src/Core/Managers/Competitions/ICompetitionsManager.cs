﻿using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Competitions
{
    public interface ICompetitionsManager
    {
        Task<PaginationModel<VideoDataModel>> GetCompetitionVideosAsync(int competitionId, int page, int pageSize);

        Task<PaginationModel<CompetitionDataModel>> GetCompetitionsAsync(int page, int pageSize);

        Task<List<CompetitionResultDataModel>> GetCompetitionResultsAsync(int id);

        Task<List<CompetitionResultDataModel>> GetCompetitionRatingsAsync(int id);
    }
}