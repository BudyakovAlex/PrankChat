using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public interface IVideoListViewModel
    {
        MvxObservableCollection<PublicationItemViewModel> Items { get; }
    }
}
