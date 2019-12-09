using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class CreateOrderViewModel : BaseViewModel
    {
        private IDialogService _dialogService;
        private DateTime? _completedDateValue;
        private string _name;
        private string _description;
        private string _price;
        private bool _isExecutorHidden;

        public CreateOrderViewModel(INavigationService navigationService,
            IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
        }

        public DateTime? CompletedDateValue
        {
            get => _completedDateValue;
            set => SetProperty(ref _completedDateValue, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public bool IsExecutorHidden
        {
            get => _isExecutorHidden;
            set => SetProperty(ref _isExecutorHidden, value);
        }

        public ICommand ShowDateDialogCommand
        {
            get => new MvxAsyncCommand(OnDateDialogCommand);
        }

        public ICommand CreateCommand
        {
            get => new MvxAsyncCommand(OnCreateCommand);
        }

        private Task OnCreateCommand()
        {
            return Task.CompletedTask;
        }

        private async Task OnDateDialogCommand()
        {
            var result = await _dialogService.ShowDateDialogAsync();
            if (result.HasValue)
            {
                CompletedDateValue = result.Value;
            }
        }
    }
}
