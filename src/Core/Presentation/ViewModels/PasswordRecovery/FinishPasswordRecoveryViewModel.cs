using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery
{
    public class FinishPasswordRecoveryViewModel : BasePageViewModel
    {
        public FinishPasswordRecoveryViewModel()
        {
            ShowLoginCommand = new MvxAsyncCommand(ShowLoginAsync);
            ShowPublicationCommand = new MvxAsyncCommand(ShowPublicationAsync);
        }

        public IMvxAsyncCommand ShowLoginCommand { get; }

        public IMvxAsyncCommand ShowPublicationCommand { get; }

        private Task ShowLoginAsync()
        {
            return NavigationManager.NavigateAsync<LoginViewModel>();
        }

        private Task ShowPublicationAsync()
        {
            return NavigationManager.NavigateAsync<MainViewModel>();
        }
    }
}
