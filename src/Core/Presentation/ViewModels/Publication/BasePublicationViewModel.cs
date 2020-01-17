using System;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class BasePublicationViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IPlatformService _platformService;
        private long _numberOfViews;
        private DateTime _publicationDate;
        private long _numberOfLikes;
        private string _shareLink;

        #region Profile

        public string ProfileName { get; set; } = "Name one";

        public string ProfilePhotoUrl { get; set; } = "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500";

        #endregion

        #region Video

        public string VideoInformationText => $"{_numberOfViews} просмотров {_publicationDate.ToShortDateString()} месяцев назад";

        public string VideoName { get; set; } = "Name video one";

        public string PlaceholderImageUrl { get; set; } = "https://ksassets.timeincuk.net/wp/uploads/sites/55/2019/04/GettyImages-1136749971-920x584.jpg";

        public string VideoUrl { get; set; }

        private bool _hasSoundTurnOn;
        public bool HasSoundTurnOn
        {
            get => _hasSoundTurnOn;
            set => SetProperty(ref _hasSoundTurnOn, value);
        }

        #endregion

        public string NumberOfLikesText => $"{Resources.Like} {_numberOfLikes}";

        #region Commands

        public MvxAsyncCommand LikeCommand => new MvxAsyncCommand(OnLikeAsync);

        public MvxAsyncCommand ShareCommand => new MvxAsyncCommand(() => _dialogService.ShowShareDialogAsync(_shareLink));

        public MvxAsyncCommand BookmarkCommand => new MvxAsyncCommand(OnBookmarkAsync);

        public MvxAsyncCommand OpenSettingsCommand => new MvxAsyncCommand(OnOpenSettingAsync);

        public MvxCommand ToggleSoundCommand => new MvxCommand(OnToggleSound);

        #endregion

        public BasePublicationViewModel(INavigationService navigationService, IDialogService dialogService)
            : base(navigationService)
        {
            _dialogService = dialogService;
        }

        public BasePublicationViewModel(INavigationService navigationService,
                                        IDialogService dialogService,
                                        IPlatformService platformService,
                                        string profileName,
                                        string profilePhotoUrl,
                                        string videoName,
                                        string videoUrl,
                                        long numberOfViews,
                                        DateTime publicationDate,
                                        long numberOfLikes,
                                        string shareLink)
            : base (navigationService)
        {
            _dialogService = dialogService;
            _platformService = platformService;

            ProfileName = profileName;
            ProfilePhotoUrl = profilePhotoUrl;
            VideoName = videoName;
            VideoUrl = videoUrl;

            _numberOfViews = numberOfViews;
            _publicationDate = publicationDate;
            _numberOfLikes = numberOfLikes;
            _shareLink = shareLink;
        }

        private Task OnLikeAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnBookmarkAsync()
        {
            return Task.CompletedTask;
        }

        private async Task OnOpenSettingAsync()
        {
            var result = await _dialogService.ShowMenuDialogAsync(new string[]
            {
                Resources.Publication_Item_Complain,
                Resources.Publication_Item_Copy_Link,
                Resources.Publication_Item_Subscribe_To_Author
            });

            if (string.IsNullOrWhiteSpace(result))
                return;

            if (result == Resources.Publication_Item_Complain)
            {
                return;
            }

            if (result == Resources.Publication_Item_Copy_Link)
            {
                await _platformService.CopyTextAsync(_shareLink);
                return;
            }

            if (result == Resources.Publication_Item_Subscribe_To_Author)
            {
                return;
            }
        }

        private void OnToggleSound()
        {
            HasSoundTurnOn = !HasSoundTurnOn;
        }
    }
}
