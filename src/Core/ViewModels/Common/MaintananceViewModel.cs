using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ViewModels.Common
{
    public class MaintananceViewModel : BasePageViewModel<string>
    {
        private string _applicationUrl;

        public MaintananceViewModel()
        {
            OpenInBrowserCommand = this.CreateCommand(OpenInBrowserAsync);
        }

        public ICommand OpenInBrowserCommand { get; }

        public override void Prepare(string parameter)
        {
            _applicationUrl = parameter;
        }

        private Task OpenInBrowserAsync()
        {
            return Browser.OpenAsync(_applicationUrl, BrowserLaunchMode.External);
        }
    }
}