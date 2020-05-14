using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace PrankChat.Mobile.Core.Providers
{
    public interface IWalkthroughsProvider
    {
        bool CheckCanShowOnFirstLoad<TViewModel>() where TViewModel : IMvxViewModel;

        Task ShowWalthroughAsync<TViewModel>() where TViewModel : IMvxViewModel; 
    }
}