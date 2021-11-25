using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels.Competition;
using PrankChat.Mobile.iOS.Views.Base;

namespace PrankChat.Mobile.iOS.Views.Competition
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class SettingsTableParticipantsView : BaseViewController<SettingsTableParticipantsViewModel>
    {
        protected override void SetupControls()
        {
            base.SetupControls();

            // TODO: Move to AppStrings.
            Title = "Настройка таблицы участников";
        }

        protected override void Bind()
        {
            using var bindingSet = CreateBindingSet();

        }
    }
}

