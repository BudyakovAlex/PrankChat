using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery
{
    public class FinishPasswordRecoveryViewModel : BasePageViewModel
    {
        public FinishPasswordRecoveryViewModel()
        {
            ShowLoginCommand = this.CreateCommand(ShowLoginAsync);
            ShowPublicationCommand = this.CreateCommand(ShowPublicationAsync);
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
