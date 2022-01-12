using PrankChat.Mobile.Core.ViewModels.Abstract;

namespace PrankChat.Mobile.Core.ViewModels.Competitions
{
    public class CompetitionRulesViewModel : BasePageViewModel<string>
    {
        public string HtmlContent { get; private set; }

        public override void Prepare(string parameter)
        {
            HtmlContent = parameter;
        }
    }
}