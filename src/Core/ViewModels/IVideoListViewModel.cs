using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ViewModels.Publication.Items;

namespace PrankChat.Mobile.Core.ViewModels
{
    public interface IVideoListViewModel
    {
        MvxObservableCollection<PublicationItemViewModel> Items { get; }
    }
}
