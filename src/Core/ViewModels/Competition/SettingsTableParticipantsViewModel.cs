using System;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.ViewModels.Abstract.Items;
using PrankChat.Mobile.Core.ViewModels.Competition.Items;

namespace PrankChat.Mobile.Core.ViewModels.Competition
{
    public class SettingsTableParticipantsViewModel : BaseItemsPageViewModel<PlaceTableParticipantsItemViewModel, int>
    {
        private const int DefaultLeftToDistributeInPercent = 100;
        private const int DefaultCountParticipants = 3;
        private const int MinimumLeftToDistributePercent = 0;

        public SettingsTableParticipantsViewModel()
        {
            AddPlaceCommand = this.CreateCommand(AddPlace);
            ApplyCommand = this.CreateCommand(ApplyAsync, CanExecuteApplyCommand);
            InitializeDefaultProperties();
        }

        public int PrizePool { get; private set; }

        private int _leftToDistribute;
        public int LeftToDistribtePercent
        {
            get => _leftToDistribute;
            set => SetProperty(ref _leftToDistribute, value);
        }

        public bool IsWarning => ApplyCommand.CanExecute();

        public IMvxCommand AddPlaceCommand { get; }
        public IMvxAsyncCommand ApplyCommand { get; }

        public override void Prepare(int parameter)
        {
            PrizePool = parameter;
        }

        private void AddPlace()
        {
            Items.Add(ProduceNewItem());

            PlaceTableParticipantsItemViewModel ProduceNewItem()
            {
                var place = Items.Count + 1;
                return new PlaceTableParticipantsItemViewModel(place, () => Math.Max(LeftToDistribtePercent, 0), PercentPlaceTableParticipantsChanged);
            }
        }

        private Task ApplyAsync()
        {
            return Task.CompletedTask;
        }

        private bool CanExecuteApplyCommand()
        {
            return LeftToDistribtePercent == MinimumLeftToDistributePercent;
        }

        private void PercentPlaceTableParticipantsChanged()
        {
            LeftToDistribtePercent = DefaultLeftToDistributeInPercent - (int)Items.Sum(item => item.Percent ?? 0);
            ApplyCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(IsWarning));
        }

        private void InitializeDefaultProperties()
        {
            for (int i = 0; i < DefaultCountParticipants; i++)
            {
                AddPlace();
            }

            LeftToDistribtePercent = DefaultLeftToDistributeInPercent;
        }
    }
}