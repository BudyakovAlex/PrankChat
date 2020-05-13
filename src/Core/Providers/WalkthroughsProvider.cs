using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace PrankChat.Mobile.Core.Providers
{
    public class WalkthroughsProvider : IWalkthroughsProvider
    {
        public WalkthroughsProvider()
        {
        }

        public bool CheckCanShowOnFirstLoad()
        {
            throw new System.NotImplementedException();
        }

        public async Task ShowWalthroughAsync<TViewModel>() where TViewModel : IMvxViewModel
        {
        }
    }
}