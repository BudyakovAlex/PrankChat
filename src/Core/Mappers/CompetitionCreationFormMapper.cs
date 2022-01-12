using System.Linq;
using PrankChat.Mobile.Core.Data.Dtos.Competitions;
using PrankChat.Mobile.Core.Data.Models.Competitions;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class CompetitionCreationFormMapper
    {
        public static CompetitionCreationFormDto Map(this CompetitionCreationForm competitionCreationForm) =>
            new CompetitionCreationFormDto
            {
                Price = competitionCreationForm.Price,
                Title = competitionCreationForm.Title,
                Description = competitionCreationForm.Description,
                EntryTax = competitionCreationForm.EntryTax,
                Type = competitionCreationForm.Type,
                EntryTaxPrizePart = competitionCreationForm.EntryTaxPrizePart,
                PrizePool = string.Join(',', competitionCreationForm.PrizePool),
                StartUploadVideosDateTime = competitionCreationForm.StartUploadVideosDateTime?.ToUniversalTime(),
                EndUploadVideosDateTime = competitionCreationForm.EndUploadVideosDateTime?.ToUniversalTime(),
                VoteToDateTime = competitionCreationForm.VoteToDateTime?.ToUniversalTime()
            };
    }
}