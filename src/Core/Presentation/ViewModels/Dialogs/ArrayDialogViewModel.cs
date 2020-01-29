using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Dialogs
{
    public class ArrayDialogViewModel : BaseViewModel, IMvxViewModel<ArrayDialogParameter, ArrayDialogResult>
    {
        public List<string> Items { get; } = new List<string>();

        public MvxCommand<string> SelectItemCommand => new MvxCommand<string>(OnSelectItem);

        public ArrayDialogViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; } = new TaskCompletionSource<object>();

        public void Prepare(ArrayDialogParameter parameter)
        {
            Items.AddRange(parameter.Items);
        }

        private void OnSelectItem(string item)
        {
            CloseCompletionSource.SetResult(new ArrayDialogResult(item));
            NavigationService.CloseView(this).FireAndForget();
        }
    }
}
