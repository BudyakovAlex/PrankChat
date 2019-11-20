using System;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class PublicationsViewModel : BaseViewModel
    {
		private PublicationType _selectedPublicationType;
		public PublicationType SelectedPublicationType
		{
			get => _selectedPublicationType;
			set => SetProperty(ref _selectedPublicationType, value);
		}

		public MvxAsyncCommand ShowNotificationCommand
		{
			get
			{
				return new MvxAsyncCommand(() => NavigationService.ShowNotificationView());
			}
		}

		public PublicationsViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
