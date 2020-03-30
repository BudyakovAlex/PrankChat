using System.Collections.Generic;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionPrizePoolViewModel : BaseViewModel, IMvxViewModel<CompetitionDataModel>
    {
        public CompetitionPrizePoolViewModel(INavigationService navigationService,
                                             IErrorHandleService errorHandleService,
                                             IApiService apiService,
                                             IDialogService dialogService,
                                             ISettingsService settingsService) : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            Items = new MvxObservableCollection<CompetitionPrizePoolItemViewModel>();
        }

        public string PrizePool { get; private set; }

        public MvxObservableCollection<CompetitionPrizePoolItemViewModel> Items { get; }

        public void Prepare(CompetitionDataModel parameter)
        {
            PrizePool = string.Format(Constants.Formats.MoneyFormat, parameter.PrizePool);
            var items = ProducePrizePoolItems(parameter.PrizePoolList);
            Items.AddRange(items);
        }

        private IEnumerable<CompetitionPrizePoolItemViewModel> ProducePrizePoolItems(List<string> prizePool)
        {
            var position = 1;
            foreach (var item in prizePool)
            {
                yield return new CompetitionPrizePoolItemViewModel(string.Format(Constants.Formats.MoneyFormat, item),
                                                                   $"{position} {Resources.Competiton_Prize_Pool_Place}",
                                                                   position);
                position++;
            }
        }
    }
}