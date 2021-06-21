using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Walthroughs
{
    public class WalthroughViewModel : BasePageViewModel<WalthroughNavigationParameter>
    {
        public string Title { get; private set; }

        public string Description { get; private set; }

        public override void Prepare(WalthroughNavigationParameter parameter)
        {
            Title = parameter.Title;
            Description = parameter.Description;
        }
    }
}