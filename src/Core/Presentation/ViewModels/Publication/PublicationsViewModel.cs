using System;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class PublicationsViewModel : BaseViewModel
    {
        public PublicationsViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
