using System;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.ViewModels.Abstract.Items;
using PrankChat.Mobile.Core.ViewModels.Competition.Items;
using PrankChat.Mobile.Core.ViewModels.Parameters;

namespace PrankChat.Mobile.Core.ViewModels.Competition
{
    public class SettingsTableParticipantsViewModel : BaseItemsPageViewModel<PlaceTableParticipantsItemViewModel, SettingsTableParticipantsNavigationParameter, PlaceTableParticipantsItemViewModel[]>
    {
        private const int DefaultLeftToDistributeInPercent = 100;
        private const int DefaultCountParticipants = 3;
        private const int MinimumLeftToDistributePercent = 0;

        public SettingsTableParticipantsViewModel()
        {
            AddPlaceCommand = this.CreateCommand(AddPlace);
            ApplyCommand = this.CreateCommand(ApplyAsync, () => LeftToDistribtePercent == MinimumLeftToDistributePercent);
            InitializeDefaultProperties();
        }

        protected override PlaceTableParticipantsItemViewModel[] DefaultResult => Items.ToArray();

        public double? PrizePool { get; private set; }

        public int LeftToDistribtePercent => Math.Max(DefaultLeftToDistributeInPercent - TotalPercentsInInput, 0);

        public int TotalPercentsInInput => (int)Items.Sum(item => item.Percent ?? 0);

        public bool IsWarning => ApplyCommand.CanExecute();

        public IMvxCommand AddPlaceCommand { get; }
        public IMvxAsyncCommand ApplyCommand { get; }

        public override void Prepare(SettingsTableParticipantsNavigationParameter parameter)
        {
            PrizePool = parameter.PrizePool;

            if (parameter.Places.IsEmpty())
            {
                return;
            }

            Items.ReplaceWith(parameter.Places);
        }

        private void AddPlace()
        {
            Items.Add(ProduceNewItem());

            PlaceTableParticipantsItemViewModel ProduceNewItem()
            {
                var place = Items.Count + 1;
                return new PlaceTableParticipantsItemViewModel(place, () => TotalPercentsInInput, PercentPlaceTableParticipantsChanged);
            }
        }

        private Task ApplyAsync()
        {
            return NavigationManager.CloseAsync(this, Items.ToArray());
        }

        private void PercentPlaceTableParticipantsChanged()
        {
            ApplyCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(IsWarning));
            RaisePropertyChanged(nameof(LeftToDistribtePercent));
        }

        private void InitializeDefaultProperties()
        {
            for (int i = 0; i < DefaultCountParticipants; i++)
            {
                AddPlace();
            }
        }
    }
}