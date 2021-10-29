using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.ViewModels.Abstract;

namespace PrankChat.Mobile.Core.ViewModels.Common
{
    public class WebViewModel : BasePageViewModel<string>
    {
        private readonly IEnvironmentConfigurationProvider _environmentConfigurationProvider;

        private string _url;
        public string Url
        {
            get => _url;
            set => SetProperty(ref _url, value, CheckUrl);
        }

        public WebViewModel(IEnvironmentConfigurationProvider environmentConfigurationProvider)
        {
            _environmentConfigurationProvider = environmentConfigurationProvider;
        }

        public override void Prepare(string parameter)
        {
            Url = parameter;
        }

        private void CheckUrl()
        {
            if (Url == _environmentConfigurationProvider.Environment.SiteUrl)
            {
                CloseCommand?.Execute(null);
            }
        }
    }
}