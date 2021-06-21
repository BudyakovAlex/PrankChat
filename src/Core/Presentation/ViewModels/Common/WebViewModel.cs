using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Common
{
    public class WebViewModel : BasePageViewModel<string>
    {
        public string Url { get; set; }

        public override void Prepare(string parameter)
        {
            Url = parameter;
        }
    }
}