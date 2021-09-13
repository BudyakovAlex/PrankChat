using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using System.Windows.Input;

namespace PrankChat.Mobile.Core.ViewModels.Registration
{
    public class RegistrationThirdStepViewModel : BasePageViewModel
    {
        public RegistrationThirdStepViewModel()
        {
            FinishRegistrationCommand = this.CreateCommand(NavigationManager.NavigateAsync<MainViewModel>);
        }

        public ICommand FinishRegistrationCommand { get; }
    }
}