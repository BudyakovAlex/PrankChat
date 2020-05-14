using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Walthroughs
{
    public class WalthroughViewModel : BaseViewModel, IMvxViewModel<WalthroughNavigationParameter>
    {
        public WalthroughViewModel(INavigationService navigationService,
                                   IErrorHandleService errorHandleService,
                                   IApiService apiService,
                                   IDialogService dialogService,
                                   ISettingsService settingsService) : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            CloseCommand = new MvxCommand(() => navigationService.CloseView(this));
        }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public ICommand CloseCommand { get; }

        public void Prepare(WalthroughNavigationParameter parameter)
        {
            Title = parameter.Title;
            Description = parameter.Description;
        }
    }
}