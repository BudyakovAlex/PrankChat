using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionDetailsViewModel : PaginationViewModel, IMvxViewModel<CompetitionApiModel>
    {
        private readonly IMvxMessenger _mvxMessenger;
        private readonly IMediaService _mediaService;

        private CompetitionApiModel _competition;
        private CompetitionDetailsHeaderViewModel _header;

        public MvxObservableCollection<BaseItemViewModel> Items { get; }

        public CompetitionDetailsViewModel(IMvxMessenger mvxMessenger,
                                           INavigationService navigationService,
                                           IErrorHandleService errorHandleService,
                                           IApiService apiService,
                                           IMediaService mediaService,
                                           IDialogService dialogService,
                                           ISettingsService settingsService) : base(Constants.Pagination.DefaultPaginationSize, navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _mvxMessenger = mvxMessenger;
            _mediaService = mediaService;

            Items = new MvxObservableCollection<BaseItemViewModel>();
        }

        public void Prepare(CompetitionApiModel parameter)
        {
            _competition = parameter;
            _header = new CompetitionDetailsHeaderViewModel(_mvxMessenger,
                                                            NavigationService,
                                                            new MvxAsyncCommand(LoadVideoAsync),
                                                            parameter);
            Items.Add(_header);
        }

        public override Task Initialize()
        {
            return LoadMoreItemsCommand.ExecuteAsync();
        }

        protected override async Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = 20)
        {
            var pageContainer = await ApiService.GetCompetitionVideosAsync(_competition.Id, page, pageSize);
            return SetList(pageContainer, page, ProduceVideoItemViewModel, Items);
        }

        private CompetitionVideoViewModel ProduceVideoItemViewModel(VideoDataModel videoDataModel)
        {
            return new CompetitionVideoViewModel(ApiService,
                                              videoDataModel.Id,
                                              videoDataModel.StreamUri,
                                              videoDataModel.User.Name,
                                              videoDataModel.User.Avatar,
                                              videoDataModel.LikesCount,
                                              videoDataModel.ViewsCount,
                                              videoDataModel.CreatedAt.UtcDateTime,
                                              videoDataModel.IsLiked,
                                              videoDataModel.User.Id == SettingsService.User.Id,
                                              _competition.GetPhase() == CompetitionPhase.Voting);
        }

        private async Task LoadVideoAsync()
        {
            try
            {
                IsBusy = true;

                var file = await _mediaService.PickVideoAsync();
                if (file == null)
                {
                    return;
                }

                var video = await ApiService.SendVideoAsync(_competition.Id, file.Path, _competition.Title, _competition.Description);
                if (video != null)
                {
                    _competition.HasLoadedVideo = true;
                    Items.Add(new CompetitionVideoViewModel(ApiService,
                                                            video.Id,
                                                            video.StreamUri,
                                                            video.User.Name,
                                                            video.User.Avatar,
                                                            video.LikesCount,
                                                            video.ViewsCount,
                                                            video.CreatedAt.UtcDateTime,
                                                            video.IsLiked,
                                                            video.User.Id == SettingsService.User.Id,
                                                            false));

                    await _header.RaisePropertyChanged(nameof(_header.CanLoadVideo));
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}