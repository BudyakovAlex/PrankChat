using System.Threading.Tasks;
using MvvmCross.Navigation;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Comment;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.Core.Presentation.ViewModels.Dialogs;
using PrankChat.Mobile.Core.Presentation.ViewModels.Notification;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Presentation.ViewModels.Video;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.Navigation
{
    public class NavigationService : INavigationService
	{
		private readonly IMvxNavigationService _mvxNavigationService;
		private readonly ISettingsService _settingsService;

		public NavigationService(IMvxNavigationService mvxNavigationService, ISettingsService settingsService)
		{
			_mvxNavigationService = mvxNavigationService;
			_settingsService = settingsService;
		}

		public Task AppStart()
		{
			if (VersionTracking.IsFirstLaunchEver || _settingsService.User != null)
			{
				return ShowMainView();
			}

			return ShowLoginView();
		}

		public Task ShowCashboxView()
		{
			return _mvxNavigationService.Navigate<CashboxViewModel>();
		}

		public Task ShowLoginView()
		{
			return _mvxNavigationService.Navigate<LoginViewModel>();
		}

		public Task ShowPasswordRecoveryView()
		{
			return _mvxNavigationService.Navigate<PasswordRecoveryViewModel>();
		}

		public Task ShowFinishPasswordRecoveryView()
		{
			return _mvxNavigationService.Navigate<FinishPasswordRecoveryViewModel>();
		}

		public Task ShowRegistrationView()
		{
			return _mvxNavigationService.Navigate<RegistrationViewModel>();
		}

		public Task ShowRegistrationSecondStepView(string email)
		{
			var parameter = new RegistrationNavigationParameter(email);
			return _mvxNavigationService.Navigate<RegistrationSecondStepViewModel, RegistrationNavigationParameter>(parameter);
		}

		public Task ShowRegistrationThirdStepView()
		{
			return _mvxNavigationService.Navigate<RegistrationThirdStepViewModel>();
		}

		public Task ShowMainView()
		{
			return _mvxNavigationService.Navigate<MainViewModel>();
		}

		public Task ShowMainViewContent()
		{
			return Task.WhenAll(
				_mvxNavigationService.Navigate<PublicationsViewModel>(),
				_mvxNavigationService.Navigate<CompetitionsViewModel>(),
				_mvxNavigationService.Navigate<CreateOrderViewModel>(),
				_mvxNavigationService.Navigate<OrdersViewModel>(),
				_mvxNavigationService.Navigate<ProfileViewModel>());
		}

		public Task ShowNotificationView()
		{
			return _mvxNavigationService.Navigate<NotificationViewModel>();
		}

		//TODO: add correct logic for opening comments
		public Task ShowCommentsView()
		{
			return _mvxNavigationService.Navigate<CommentsViewModel>();
		}

		public Task ShowCashboxContent()
		{
			return Task.WhenAll(
				_mvxNavigationService.Navigate<RefillViewModel>(),
						   _mvxNavigationService.Navigate<WithdrawalViewModel>());
		}

		public Task<bool> CloseView(BaseViewModel viewModel)
		{
			return _mvxNavigationService.Close(viewModel);
		}

		public Task ShowSearchView()
		{
			return _mvxNavigationService.Navigate<SearchViewModel>();
		}

		public Task<OrderDetailsResult> ShowOrderDetailsView(int orderId)
		{
			var parameter = new OrderDetailsNavigationParameter(orderId);
			return _mvxNavigationService.Navigate<OrderDetailsViewModel, OrderDetailsNavigationParameter, OrderDetailsResult>(parameter);
		}

		public Task ShowFullScreenVideoView(FullScreenVideoParameter fullScreenVideoParameter)
		{
            if (string.IsNullOrEmpty(fullScreenVideoParameter.VideoUrl))
            {
				return Task.CompletedTask;
            }

			return _mvxNavigationService.Navigate<FullScreenVideoViewModel, FullScreenVideoParameter>(fullScreenVideoParameter);
		}

		public Task ShowDetailsPublicationView()
		{
			return _mvxNavigationService.Navigate<PublicationDetailsViewModel>();
		}

		public Task ShowWithdrawalView()
		{
			var navigationParameter = new CashboxTypeNavigationParameter(CashboxTypeNavigationParameter.CashboxType.Withdrawal);
			return _mvxNavigationService.Navigate<CashboxViewModel, CashboxTypeNavigationParameter>(navigationParameter);
		}

		public Task ShowRefillView()
		{
			var navigationParameter = new CashboxTypeNavigationParameter(CashboxTypeNavigationParameter.CashboxType.Refill);
			return _mvxNavigationService.Navigate<CashboxViewModel, CashboxTypeNavigationParameter>(navigationParameter);
		}

		public async Task<bool> ShowUpdateProfileView()
		{
			var result = await _mvxNavigationService.Navigate<ProfileUpdateViewModel, ProfileUpdateResult>();
			return (result?.IsProfileUpdated ?? false) || (result?.IsAvatarUpdated ?? false);
		}

		public Task Logout()
		{
			return ShowLoginView();
		}

		public Task ShowWebView(string url)
		{
			var parameter = new WebViewNavigationParameter(url);
			return _mvxNavigationService.Navigate<WebViewModel, WebViewNavigationParameter>(parameter);
		}

		public Task ShowProfileUser(int idUser)
		{
			return Task.FromResult(0);
			// TODO change this code for normal navigate to profile other user
			// return _mvxNavigationService.Navigate<ProfileViewModel, int>(idUser);
		}

		public Task<ImageCropPathResult> ShowImageCropView(string filePath)
		{
			var parameter = new ImagePathNavigationParameter(filePath);
			return _mvxNavigationService.Navigate<ImageCropViewModel, ImagePathNavigationParameter, ImageCropPathResult>(parameter);
		}

		#region Dialogs

		public Task ShowShareDialog(ShareDialogParameter parameter)
		{
			return _mvxNavigationService.Navigate<ShareDialogViewModel, ShareDialogParameter>(parameter);
		}

		public Task<ArrayDialogResult> ShowArrayDialog(ArrayDialogParameter parameter)
		{
			return _mvxNavigationService.Navigate<ArrayDialogViewModel, ArrayDialogParameter, ArrayDialogResult>(parameter);
		}

        public Task ShowCompetitionDetailsView(CompetitionDataModel competition)
        {
			return _mvxNavigationService.Navigate<CompetitionDetailsViewModel, CompetitionDataModel>(competition);
		}

		public Task ShowCompetitionPrizePoolView(CompetitionDataModel competition)
        {
			return _mvxNavigationService.Navigate<CompetitionPrizePoolViewModel, CompetitionDataModel>(competition);
		}

		public Task ShowCompetitionRulesView(string content)
        {
			return _mvxNavigationService.Navigate<CompetitionRulesViewModel, string>(content);
		}

		#endregion
	}
}
