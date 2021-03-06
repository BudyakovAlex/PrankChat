using MvvmCross.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class RegistrationThirdStepViewModel : BasePageViewModel
    {
        public RegistrationThirdStepViewModel()
        {
            FinishRegistrationCommand = this.CreateCommand(FinishRegistrationAsync);
        }

        public MvxAsyncCommand FinishRegistrationCommand { get; }

        private Task FinishRegistrationAsync()
        {
            return NavigationManager.NavigateAsync<MainViewModel>();
        }
    }
}