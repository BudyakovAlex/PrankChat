using MvvmCross.Commands;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.ViewModels.Abstract.Items;
using PrankChat.Mobile.Core.ViewModels.Competition.Items;

namespace PrankChat.Mobile.Core.ViewModels.Competition
{
    public class SettingsTableParticipantsViewModel : BaseItemsPageViewModel<PlaceTableParticipantsItemViewModel, int>
    {
        private int _fullPrizePool;

        public SettingsTableParticipantsViewModel()
        {
            AddPlaceCommand = this.CreateCommand(AddPlace);
        }

        public IMvxCommand AddPlaceCommand { get; }

        public override void Prepare(int parameter)
        {
            _fullPrizePool = parameter;
        }

        private void AddPlace()
        {
            Items.Add(new PlaceTableParticipantsItemViewModel(Items.Count + 1));
        }
    }
}