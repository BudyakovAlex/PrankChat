using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public class WebViewModel : BasePageViewModel, IMvxViewModel<WebViewNavigationParameter>
    {
        public string Url { get; set; }

        public void Prepare(WebViewNavigationParameter parameter)
        {
            Url = parameter.Url;
        }
    }
}
