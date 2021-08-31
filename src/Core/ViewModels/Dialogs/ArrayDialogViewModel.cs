﻿using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.Core.ViewModels.Parameters;
using PrankChat.Mobile.Core.ViewModels.Results;

namespace PrankChat.Mobile.Core.ViewModels.Dialogs
{
    public class ArrayDialogViewModel : BasePageViewModel, IMvxViewModel<ArrayDialogParameter, ArrayDialogResult>
    {
        private string _selectedItem;

        public ArrayDialogViewModel()
        {
            SelectItemCommand = this.CreateCommand<string>(SelectItemAsync);
            DoneCommand = this.CreateCommand(DoneAsync);
            Items = new MvxObservableCollection<string>();
        }

        public IMvxAsyncCommand<string> SelectItemCommand { get; }

        public IMvxAsyncCommand DoneCommand { get; }

        public string Title { get; private set; }

        public MvxObservableCollection<string> Items { get; }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; } =
            new TaskCompletionSource<object>();

        public string SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource != null
                              && !CloseCompletionSource.Task.IsCompleted
                              && !CloseCompletionSource.Task.IsFaulted)
            {
                CloseCompletionSource?.TrySetCanceled();
            }

            base.ViewDestroy(viewFinishing);
        }

        public void Prepare(ArrayDialogParameter parameter)
        {
            Items.AddRange(parameter.Items);

            SelectedItem = Items.FirstOrDefault();
            Title = parameter.Title;
        }

        private Task SelectItemAsync(string item)
            => PickItemAsync(item);

        private Task DoneAsync()
            => PickItemAsync(SelectedItem);

        private async Task PickItemAsync(string value)
        {
            await NavigationManager.CloseAsync(this, new ArrayDialogResult(value));
        }
    }
}
