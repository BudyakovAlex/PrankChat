using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionPrizePoolViewModel : BaseViewModel, IMvxViewModel<int>
    {
        private int _competitionId;

        public CompetitionPrizePoolViewModel(INavigationService navigationService,
                                             IErrorHandleService errorHandleService,
                                             IApiService apiService,
                                             IDialogService dialogService,
                                             ISettingsService settingsService) : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            Items = new ObservableCollection<CompetitionPrizePoolItemViewModel>();
        }

        public string PrizePool { get; private set; }

        public ObservableCollection<CompetitionPrizePoolItemViewModel> Items { get; }

        //TODO: add loading logic here
        public override Task Initialize()
        {
            PrizePool = "1000000$";

            RaisePropertyChanged(nameof(PrizePool));
            return base.Initialize();
        }

        public void Prepare(int parameter)
        {
            _competitionId = parameter;
        }
    }
}