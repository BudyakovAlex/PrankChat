using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public class MaintananceViewModel : BaseViewModel, IMvxViewModel<string>
    {
        private string _applicationUrl;

        public MaintananceViewModel()
        {
            OpenInBrowserCommand = new MvxAsyncCommand(OpenInBrowserAsync);
        }

        public IMvxAsyncCommand OpenInBrowserCommand { get; }

        public void Prepare(string parameter)
        {
            _applicationUrl = parameter;
        }

        private Task OpenInBrowserAsync()
        {
            return Browser.OpenAsync(_applicationUrl, BrowserLaunchMode.External);
        }
    }
}