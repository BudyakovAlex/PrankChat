using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Common
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