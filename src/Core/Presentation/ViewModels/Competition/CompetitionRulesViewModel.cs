using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionRulesViewModel : BaseViewModel, IMvxViewModel<string>
    {
        public string HtmlContent { get; private set; }

        public void Prepare(string parameter)
        {
            HtmlContent = parameter;
        }
    }
}