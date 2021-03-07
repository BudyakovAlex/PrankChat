using MvvmCross.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class RegistrationThirdStepViewModel : BasePageViewModel
    {
        public RegistrationThirdStepViewModel()
        {
            FinishRegistrationCommand = this.CreateCommand(NavigationManager.NavigateAsync<MainViewModel>);
        }

        public MvxAsyncCommand FinishRegistrationCommand { get; }
    }
}