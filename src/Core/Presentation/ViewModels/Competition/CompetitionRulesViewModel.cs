using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionRulesViewModel : BasePageViewModel, IMvxViewModel<string>
    {
        public string HtmlContent { get; private set; }

        public void Prepare(string parameter)
        {
            HtmlContent = parameter;
        }
    }
}