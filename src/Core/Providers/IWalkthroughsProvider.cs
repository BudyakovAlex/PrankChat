using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace PrankChat.Mobile.Core.Providers
{
    public interface IWalkthroughsProvider
    {
        bool CheckCanShowOnFirstLoad();

        Task ShowWalthroughAsync<TViewModel>() where TViewModel : IMvxViewModel; 
    }
}