using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery
{
    public class FinishPasswordRecoveryViewModel : BaseViewModel
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
            return NavigationService.ShowLoginView();
        }

        private Task ShowPublicationAsync()
        {
            return NavigationService.ShowMainView();
        }
    }
}
