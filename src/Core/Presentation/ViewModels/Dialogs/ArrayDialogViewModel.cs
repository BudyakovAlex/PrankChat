using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Dialogs
{
    public class ArrayDialogViewModel : BaseViewModel, IMvxViewModel<ArrayDialogParameter, ArrayDialogResult>
    {
        public string Title { get; set; }

        public List<string> Items { get; } = new List<string>();

        private string _selectedItem;
        public string SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; } = new TaskCompletionSource<object>();

        public MvxCommand<string> SelectItemCommand => new MvxCommand<string>(OnSelectItem);

        public ArrayDialogViewModel(INavigationService navigationService,
                                    IErrorHandleService errorHandleService,
                                    IApiService apiService,
                                    IDialogService dialogService,
                                    ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
        }

        public void Prepare(ArrayDialogParameter parameter)
        {
            Items.AddRange(parameter.Items);
            SelectedItem = Items.FirstOrDefault();
            Title = parameter.Title;
        }

        private void OnSelectItem(string item)
        {
            if (string.IsNullOrWhiteSpace(item))
                item = SelectedItem;

            CloseCompletionSource.SetResult(new ArrayDialogResult(item));
            NavigationService.CloseView(this).FireAndForget();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
                CloseCompletionSource?.TrySetCanceled();

            base.ViewDestroy(viewFinishing);
        }
    }
}
