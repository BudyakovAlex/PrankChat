using System;
using Acr.UserDialogs;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class RegistrationSecondStepViewModel : BaseViewModel
    {
        private DateTime _birthday;
        public DateTime Birthday
        {
            get => _birthday;
            set => SetProperty(ref _birthday, value);
        }

        public MvxAsyncCommand ShowThirdStepCommand
        {
            get
            {
                return new MvxAsyncCommand(() => NavigationService.ShowRegistrationThirdStepView());
            }
        }

        public MvxAsyncCommand SelectBirthday
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    var birthday = await UserDialogs.Instance.DatePromptAsync("kik");

                });
            }
        }

        public RegistrationSecondStepViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
