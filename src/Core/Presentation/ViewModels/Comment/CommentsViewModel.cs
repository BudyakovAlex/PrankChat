using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Comment.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Comment
{
    public class CommentsViewModel : BaseViewModel
    {
        public MvxObservableCollection<CommentItemViewModel> Items { get; } = new MvxObservableCollection<CommentItemViewModel>();

        public string Comment { get; set; }

        public MvxAsyncCommand SendCommentCommand => new MvxAsyncCommand(OnSendCommentAsync);

        public CommentsViewModel(INavigationService navigationService,
                                 IErrorHandleService errorHandleService,
                                 IApiService apiService,
                                 IDialogService dialogService,
                                 ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
        }

        public override Task Initialize()
        {
            Items.Add(new CommentItemViewModel("Name one", "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500", "First comment", new DateTime(2019, 1, 13)));
            Items.Add(new CommentItemViewModel("Name two", "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500", "Second comment", new DateTime(2019, 4, 18)));
            return base.Initialize();
        }

        private Task OnSendCommentAsync()
        {
            return Task.CompletedTask;
        }
    }
}
