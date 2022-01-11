using PrankChat.Mobile.Core.ViewModels.Competition.Items;

namespace PrankChat.Mobile.Core.ViewModels.Parameters
{
    public class SettingsTableParticipantsNavigationParameter
    {
        public SettingsTableParticipantsNavigationParameter(PlaceTableParticipantsItemViewModel[] places, double? prizePool)
        {
            Places = places;
            PrizePool = prizePool;
        }

        public PlaceTableParticipantsItemViewModel[] Places { get; }

        public double? PrizePool { get; }
    }
}
